using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Com.Ittianyu.Bottomnavigationviewex;
using Android.Support.Design.Internal;

namespace Naxam.Controls.Platform.Droid.Utils
{
    using RelativeLayout = Android.Widget.RelativeLayout;
    using Platform = Xamarin.Forms.Platform.Android.Platform;

    public static class BottomTabbedRendererUtils
    {
        public static Rectangle CreateRect(this Context context, int width, int height)
        {
            return new Rectangle(
                    0, 0,
                    context.FromPixels(width),
                    context.FromPixels(height)
                );
        }

        public static void HandlePagesChanged(this BottomTabbedRenderer renderer)
        {
            renderer.SetupBottomBar();
            renderer.SetupTabItems();

            if (renderer.Element.Children.Count == 0)
            {
                return;
            }

            EnsureTabIndex(renderer);
        }

        static void EnsureTabIndex(BottomTabbedRenderer renderer)
        {
            var rootLayout = (RelativeLayout)renderer.GetChildAt(0);
            var bottomNav = (BottomNavigationViewEx)rootLayout.GetChildAt(1);
            var menu = (BottomNavigationMenu)bottomNav.Menu;

            var itemIndex = menu.FindItemIndex(renderer.CurrentMenuItemId);
            var pageIndex = renderer.Element.Children.IndexOf(renderer.Element.CurrentPage);
            if (pageIndex >= 0 && pageIndex != itemIndex && pageIndex < bottomNav.ItemCount)
            {
                var menuItem = menu.GetItem(pageIndex);
                bottomNav.SelectedItemId = menuItem.ItemId;

                if (BottomTabbedRenderer.ShouldUpdateSelectedIcon && BottomTabbedRenderer.MenuItemIconSetter != null)
                {
                    BottomTabbedRenderer.MenuItemIconSetter?.Invoke(menuItem, renderer.Element.CurrentPage.IconImageSource as FileImageSource, true);

                    if (renderer.LastSelectedIndex != pageIndex)
                    {
                        var lastSelectedPage = renderer.Element.Children[renderer.LastSelectedIndex];
                        var lastSelectedMenuItem = menu.GetItem(renderer.LastSelectedIndex);
                        BottomTabbedRenderer.MenuItemIconSetter?.Invoke(lastSelectedMenuItem, lastSelectedPage.IconImageSource as FileImageSource, false);
                        renderer.LastSelectedIndex = pageIndex;
                    }
                }
                else if (renderer.LastSelectedIndex != pageIndex)
                {
                    renderer.LastSelectedIndex = pageIndex;
                }
            }
        }

        public static void SwitchPage(this BottomTabbedRenderer renderer, IMenuItem item)
        {
            var rootLayout = (RelativeLayout)renderer.GetChildAt(0);
            var bottomNav = (BottomNavigationViewEx)rootLayout.GetChildAt(1);
            var menu = (BottomNavigationMenu)bottomNav.Menu;
            renderer.CurrentMenuItemId = item.ItemId;
            var index = menu.FindItemIndex(item.ItemId);
            var pageIndex = index % renderer.Element.Children.Count;
            var currentPageIndex = renderer.Element.Children.IndexOf(renderer.Element.CurrentPage);

            if (pageIndex == currentPageIndex)
            {
                if (renderer.Element.CurrentPage is NavigationPage nav)
                {
                    nav.Navigation.PopToRootAsync();
                }
            }
            else
            {
                renderer.Element.CurrentPage = renderer.Element.Children[pageIndex];
            }
        }

        public static void Layout(this BottomTabbedRenderer renderer, int width, int height)
        {
            var rootLayout = (RelativeLayout)renderer.GetChildAt(0);
            var bottomNav = (BottomNavigationViewEx)rootLayout.GetChildAt(1);

            var Context = renderer.Context;

            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            ((IPageController)renderer.Element).ContainerArea = Context.CreateRect(rootLayout.MeasuredWidth, rootLayout.GetChildAt(0).MeasuredHeight);

            rootLayout.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
            rootLayout.Layout(0, 0, rootLayout.MeasuredWidth, rootLayout.MeasuredHeight);

            if (renderer.Element.Children.Count == 0)
            {
                return;
            }

            int tabsHeight = bottomNav.MeasuredHeight;

            var item = (ViewGroup)bottomNav.GetChildAt(0);

            item.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));

            item.Layout(0, 0, width, tabsHeight);

            return;
        }

        [Obsolete]
        public static void SetupTabItems(this BottomTabbedRenderer renderer, BottomNavigationViewEx bottomNav)
        {
            var Element = renderer.Element;
            var menu = (BottomNavigationMenu)bottomNav.Menu;
            menu.Clear();
            var mPresenterField = Java.Lang.Class.FromType(typeof(BottomNavigationMenuView)).GetDeclaredField("presenter");
            mPresenterField.Accessible = true;
            var mPresenter = (BottomNavigationPresenter)mPresenterField.Get(bottomNav.BottomNavigationMenuView);
            mPresenterField.Accessible = false;

            if (Element.Children.Count == 0)
            {
                return;
            }

            mPresenter.SetUpdateSuspended(true);
            var tabsCount = Math.Min(Element.Children.Count, bottomNav.MaxItemCount);
            for (int i = 0; i < tabsCount; i++)
            {
                var page = Element.Children[i];
                var menuItem = menu.Add(0, i, 0, page.Title);
                var setter = BottomTabbedRenderer.MenuItemIconSetter ?? BottomTabbedRenderer.DefaultMenuItemIconSetter;
                setter.Invoke(menuItem, page.IconImageSource as FileImageSource, renderer.LastSelectedIndex == i);
            }
            mPresenter.SetUpdateSuspended(false);
            mPresenter.UpdateMenuView(true);
            //Deprecated use {@link #setLabelVisibilityMode }
            bottomNav.EnableShiftingMode(false);//enable It will has a shift animation if true. Otherwise all items are the same width.
            //Deprecated use {@link #setItemHorizontalTranslationEnabled(boolean)}
            bottomNav.EnableItemShiftingMode(false);//enable It will has a shift animation for item if true. Otherwise the item text always be shown.
            bottomNav.EnableAnimation(false);//enable It means the text won't scale and icon won't move when active it in no item shifting mode if false.
            bottomNav.SetTextVisibility(BottomTabbedRenderer.VisibleTitle.HasValue ? BottomTabbedRenderer.VisibleTitle.Value : true);
            if (BottomTabbedRenderer.Typeface != null)
            {
                bottomNav.SetTypeface(BottomTabbedRenderer.Typeface);
            }
            if (BottomTabbedRenderer.IconSize.HasValue)
            {
                bottomNav.SetIconSize(BottomTabbedRenderer.IconSize.Value, BottomTabbedRenderer.IconSize.Value);
            }
            if (BottomTabbedRenderer.FontSize.HasValue)
            {
                bottomNav.SetTextSize(BottomTabbedRenderer.FontSize.Value);
            }

            bottomNav.TextAlignment = Android.Views.TextAlignment.Center;
        }

        public static BottomNavigationViewEx SetupBottomBar(this BottomTabbedRenderer renderer, Android.Widget.RelativeLayout rootLayout, BottomNavigationViewEx bottomNav, int barId)
        {
            if (bottomNav != null)
            {
                rootLayout.RemoveView(bottomNav);
                bottomNav.SetOnNavigationItemSelectedListener(null);
            }

            var barParams = new Android.Widget.RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                BottomTabbedRenderer.BottomBarHeight.HasValue ? (int)rootLayout.Context.ToPixels(BottomTabbedRenderer.BottomBarHeight.Value) : ViewGroup.LayoutParams.WrapContent);
            //Add alignment rules to close to the lower edge of the parent contro
            barParams.AddRule(LayoutRules.AlignParentBottom);
            bottomNav = new BottomNavigationViewEx(rootLayout.Context)
            {
                LayoutParameters = barParams,
                Id = barId
            };
            if (BottomTabbedRenderer.BackgroundColor.HasValue)
            {
                bottomNav.SetBackgroundColor(BottomTabbedRenderer.BackgroundColor.Value);
            }
            if (BottomTabbedRenderer.ItemIconTintList != null)
            {
                bottomNav.ItemIconTintList = BottomTabbedRenderer.ItemIconTintList;
            }
            if (BottomTabbedRenderer.ItemTextColor != null)
            {
                bottomNav.ItemTextColor = BottomTabbedRenderer.ItemTextColor;
            }
            if (BottomTabbedRenderer.ItemBackgroundResource.HasValue)
            {
                bottomNav.ItemBackgroundResource = BottomTabbedRenderer.ItemBackgroundResource.Value;
            }
            //The listening callback method "setOnNavigationItemSelectedListener()", which defaults to the first entry being the first item selected, can be set by 
            //"navigation.getMenu().getItem(index).setChecked(true);" 
            bottomNav.SetOnNavigationItemSelectedListener(renderer);
            rootLayout.AddView(bottomNav, 1, barParams);

            return bottomNav;
        }

        public static void ChangePage(this BottomTabbedRenderer renderer, FrameLayout pageContainer, Page page)
        {
            renderer.Context.HideKeyboard(renderer);

            if (page == null)
            {
                return;
            }

            if (Platform.GetRenderer(page) == null)
            {
                Platform.SetRenderer(page, Platform.CreateRendererWithContext(page, renderer.Context));
            }
            var pageContent = Platform.GetRenderer(page).View;
            pageContainer.AddView(pageContent);
            if (pageContainer.ChildCount > 1)
            {
                pageContainer.RemoveViewAt(0);
            }

            EnsureTabIndex(renderer);
        }

        public static RelativeLayout CreateRoot(this BottomTabbedRenderer renderer, int barId, int pageContainerId, out FrameLayout pageContainer)
        {
            var rootLayout = new RelativeLayout(renderer.Context)
            {
                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
            };
            var pageParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            pageParams.AddRule(LayoutRules.Above, barId);

            pageContainer = new FrameLayout(renderer.Context)
            {
                LayoutParameters = pageParams,
                Id = pageContainerId
            };

            rootLayout.AddView(pageContainer, 0, pageParams);

            return rootLayout;
        }
    }
}