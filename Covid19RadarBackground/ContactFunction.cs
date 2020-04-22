using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Covid19Radar.DataStore;
using Covid19Radar.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Covid19Radar
{
    public class ContactFunction
    {
        private readonly ICosmos Cosmos;
        private readonly IStoringCosmos Store;
        private readonly ILogger<ContactFunction> Logger;

        public ContactFunction(ICosmos cosmos, IStoringCosmos store, ILogger<ContactFunction> logger)
        {
            Cosmos = cosmos;
            Store = store;
            Logger = logger;
        }

        [FunctionName("ContactFunction")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "EXAMPLE",
            collectionName: "Beacons",
            ConnectionStringSetting = "COSMOS_CONNECTION",
            LeaseCollectionName = "Lease",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Microsoft.Azure.Documents.Document> input)
        {
            Logger.LogInformation($"{nameof(ContactFunction)} processed a request.");
            foreach (var i in input)
            {
                var d = JsonConvert.DeserializeObject<BeaconModel>(i.ToString());
                Logger.LogInformation($"{nameof(ContactFunction)}  Change feed Major:{d.UserMajor} Minor:{d.UserMinor}");
                var p = await QueryPair(d);
                if (p == null)
                {
                    Logger.LogInformation($"{nameof(ContactFunction)} miss match Major:{d.Major} Minor:{d.Minor}");
                    continue;
                }
                BeaconModel b1, b2;
                IUserMajorMinorExtension.SetDecideLeftRight(d, p, out b1, out b2);
                await Upsert(b1, b2);
            }
        }

        private async Task<BeaconModel> QueryPair(BeaconModel input)
        {
            // pair
            var queryPair = new QueryDefinition
                ($"SELECT * FROM {Cosmos.ContainerNameBeacon} b" 
                + " WHERE b.UserMajor = @UserMajor" 
                + " AND b.UserMinor = @UserMinor"
                + " AND b.KeyTime = @KeyTime"
                + " AND b.Major = @Major"
                + " AND b.Minor = @Minor")
                .WithParameter("@UserMajor", input.Major)
                .WithParameter("@UserMinor", input.Minor)
                .WithParameter("@KeyTime", input.KeyTime)
                .WithParameter("@Major", input.UserMajor)
                .WithParameter("@Minor", input.UserMinor);
            var option = new QueryRequestOptions();
            option.PartitionKey = new PartitionKey($"{input.Major}.{input.Minor}");
            try
            {
                var pair = await Cosmos.Beacon.GetItemQueryIterator<BeaconModel>(queryPair, null, option).ReadNextAsync();
                return pair.FirstOrDefault();
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Logger.LogInformation($"{nameof(ContactFunction)} Not Found Major:{input.Major} Minor:{input.Major}");
                    return null;
                }
                Logger.LogError(ex, $"{nameof(ContactFunction)} Throw from QueryPair Major:{input.Major} Minor:{input.Major}");
            }
            return null;
        }

        private async Task Upsert(BeaconModel b1, BeaconModel b2)
        {
            var item = new ContactModel();
            var pk = $"{b1.UserMajor}.{b1.UserMinor}-{b2.UserMajor}.{b2.UserMinor}";
            item.id = $"{b1.UserMajor}.{b1.UserMinor}-{b1.KeyTime}-{b2.UserMajor}.{b2.UserMinor}";
            item.KeyTime = b1.KeyTime;
            item.PartitionKey = pk;
            item.Beacon1 = b1;
            item.Beacon2 = b2;
            Logger.LogInformation($"{nameof(ContactFunction)} Upsert id:{item.id}");
            try
            {
                var result = await Store.Contact.UpsertItemAsync(item, new PartitionKey(pk));
                Logger.LogInformation($"{nameof(ContactFunction)} Complete Upsert id:{item.id}");
            }
            catch (CosmosException ex)
            {
                Logger.LogError(ex, $"{nameof(ContactFunction)} Throw from Upsert id:{item.id}");
            }

        }
    }
}
