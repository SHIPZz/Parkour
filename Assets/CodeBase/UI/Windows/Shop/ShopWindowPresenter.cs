﻿using System;
using CodeBase.Enums;
using CodeBase.Services.Pause;
using CodeBase.Services.SaveSystems;
using CodeBase.Services.SaveSystems.Data;
using Zenject;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindowPresenter : IInitializable, IDisposable
    {
        private readonly ShopView _shopView;
        private readonly WindowService _windowService;
        private readonly ShopMoneyText _shopMoneyText;
        private readonly Wallet.Wallet _wallet;
        private readonly IPauseService _pauseService;

        public ShopWindowPresenter(ShopView shopView, WindowService windowService,
            ShopMoneyText shopMoneyText, Wallet.Wallet wallet,
            IPauseService pauseService)
        {
            _pauseService = pauseService;
            _wallet = wallet;
            _shopMoneyText = shopMoneyText;
            _shopView = shopView;
            _windowService = windowService;
        }

        public void Initialize()
        {
            _shopView.OpenedButtonClicked += Open;
            _shopView.ClosedButtonClicked += Close;
            _wallet.MoneyChanged += _shopMoneyText.SetMoney;
        }

        public void Dispose()
        {
            _wallet.MoneyChanged -= _shopMoneyText.SetMoney;
            _shopView.OpenedButtonClicked -= Open;
            _shopView.ClosedButtonClicked -= Close;
        }

        private void Open()
        {
            _shopMoneyText.SetMoney(_wallet.Money);
            _windowService.CloseAll(() => _windowService.Open(WindowTypeId.Shop));
        }

        private void Close() => 
            _windowService.CloseAll(() => _windowService.Open(WindowTypeId.Play,_pauseService.UnPause));
    }
}