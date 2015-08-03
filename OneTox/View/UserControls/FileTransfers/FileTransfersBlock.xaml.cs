﻿using System.Collections.Specialized;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using OneTox.ViewModel.FileTransfers;

namespace OneTox.View.UserControls.FileTransfers
{
    public sealed partial class FileTransfersBlock : UserControl
    {
        private FileTransfersViewModel _viewModel;

        public FileTransfersBlock()
        {
            InitializeComponent();
        }

        private async void FileTransferBlockLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as FileTransfersViewModel;
            VisualStateManager.GoToState(this, _viewModel.VisualStates.BlockState.ToString(), true);
            _viewModel.Transfers.CollectionChanged += TransfersCollectionChangedHandler;
            await SetAddDeleteThemeTransitionForTransferRibbons();
        }

        private void TransfersCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ScrollTransferRibbonsToBottom();
            }
        }

        private void ScrollTransferRibbonsToBottom()
        {
            TransferRibbonsScrollViewer.UpdateLayout();
            TransferRibbonsScrollViewer.ChangeView(null, double.MaxValue, null, true);
        }

        private async Task SetAddDeleteThemeTransitionForTransferRibbons()
        {
            // We need this ugly hack because otherwise every time we navigate to ChatPage
            // we'd see the "Add" animation of every item in the list (and we do not want that).
            await Task.Delay(1);
            TransferRibbons.ItemContainerTransitions = new TransitionCollection {new AddDeleteThemeTransition()};
        }

        private void ShowTransfersIconTapped(object sender, TappedRoutedEventArgs e)
        {
            _viewModel.VisualStates.BlockState =
                FileTransfersViewModel.FileTransfersVisualStates.TransfersBlockState.Open;
        }

        private void HideTransfersIconTapped(object sender, TappedRoutedEventArgs e)
        {
            _viewModel.VisualStates.BlockState =
                FileTransfersViewModel.FileTransfersVisualStates.TransfersBlockState.Collapsed;
        }
    }
}