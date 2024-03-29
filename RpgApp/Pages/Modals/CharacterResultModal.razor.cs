﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.Modals
{
    public partial class CharacterResultModal
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }

        protected Player CurrentPlayer { get; set; }

        protected override Task OnInitializedAsync()
        {
            CurrentPlayer = AppState.CurrentPlayer;
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AppState.CurrentPlayer)) return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(CharacterResultModal)}");
            CurrentPlayer = AppState.CurrentPlayer;
            StateHasChanged();

        }
        public void BeginWindow()
        {
            ModalService.Close(true);
        }
    }
}
