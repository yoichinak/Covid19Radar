﻿using System.Collections.Generic;
using Covid19Radar.Model;
using Prism.Navigation;
using Xamarin.Forms;

namespace Covid19Radar.ViewModels
{
    public class DescriptionPageViewModel : ViewModelBase
    {
        public List<StepModel> Steps { get; set; }

        public DescriptionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Resx.AppResources.TitleHowItWorks;
            SetData();
        }

        public Command OnClickNext => new Command(() => NavigationService.NavigateAsync("ConsentByUserPage"));

        private void SetData()
        {
            Steps = new List<StepModel>
            {
                new StepModel
                {
                    Title = Resx.AppResources.TextStep1Title,
                    Image = "descriptionStep1.png",
                    Description = Resx.AppResources.TextStep1Description,
                    StepNumber = 1
                },
                new StepModel
                {
                    Title = Resx.AppResources.TextStep2Title,
                    Image = "descriptionStep2.png",
                    Description = Resx.AppResources.TextStep2Description,
                    StepNumber = 2
                },
                new StepModel
                {
                    Title = Resx.AppResources.TextStep3Title,
                    Image = "descriptionStep3.png",
                    Description = Resx.AppResources.TextStep3Description,
                    StepNumber = 3
                },
                new StepModel
                {
                    Title = Resx.AppResources.TextStep4Title,
                    Image = "descriptionStep4.png",
                    Description = Resx.AppResources.TextStep4Description
                }
            };

        }
    }
}
