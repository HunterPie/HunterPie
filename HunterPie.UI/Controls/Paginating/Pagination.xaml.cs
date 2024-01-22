using HunterPie.UI.Controls.Paginating.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Paginating;
/// <summary>
/// Interaction logic for Pagination.xaml
/// </summary>
public partial class Pagination : UserControl
{
    public ObservableCollection<PaginationItemViewModel> Pages { get; } = new();

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(Pagination), new PropertyMetadata(0, OnTotalPagesChange));

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination), new PropertyMetadata(0));

    public Pagination()
    {
        InitializeComponent();
    }

    private static void OnTotalPagesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Pagination pagination)
            return;

        int totalPages = pagination.TotalPages;
        int currentPage = pagination.CurrentPage;

        Span<int> availablePages = stackalloc int[5] { 0, currentPage - 1, currentPage, currentPage + 1, totalPages - 1 };

        List<PaginationItemViewModel> pageBuffer = new(Math.Min(7, totalPages));
        int lastPageAdded = int.MinValue;
        foreach (int page in availablePages)
        {
            if (page < 0 || page == lastPageAdded)
                continue;

            bool hasSkippedPages = page - lastPageAdded > 1;

            if (hasSkippedPages)
                pageBuffer.Add(new PaginationItemViewModel
                {
                    IsGap = true
                });

            pageBuffer.Add(new PaginationItemViewModel
            {
                IsActive = page == currentPage,
                Page = page + 1
            });

            lastPageAdded = page;
        }

        pagination.Pages.Clear();
        pageBuffer.ForEach(it => pagination.Pages.Add(it));
    }
}
