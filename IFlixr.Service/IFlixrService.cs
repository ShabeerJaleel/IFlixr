using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel;

namespace IFlixr.Service
{
    public interface IFlixrService
    {
        BannerModel GetHomeScreenBanner();

        HomePageOld GetHomePage();

        MoviePageOld GetMoviePage(string id);

        DynamicLayout GetDynamicLayout(string id, string imageSizeClass = "image-medium");

        MovieBrowsePageOld GetMovieBrowsePage(string id);
    }
}
