﻿using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Images;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.CustomComponents
{
    public partial class ImageSelectionComponent
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private ImageModel[] AllImages { get; set; }
        [Parameter]
        public Action<string> OnImageSelected { get; set; }
        protected async override Task OnInitializedAsync()
        {
            try
            {
                this.AuthorizedHttpClient = this.HttpClientService.CreatedAuthorizedClient();
                this.AllImages = await this.AuthorizedHttpClient.GetFromJsonAsync<ImageModel[]>("api/Image/ListImages");
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {

            }
        }

        private void SelectImage(ImageModel imageModel)
        {
            OnImageSelected(imageModel.ImageUrl);
        }
    }
}
