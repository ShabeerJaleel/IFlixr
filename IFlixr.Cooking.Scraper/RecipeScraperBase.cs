using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Cooking.Data;
using CsQuery;

namespace IFlixr.Cooking.Scraper
{
    public abstract class RecipeScraperBase
    {
        #region Fields

        private static readonly GotoUrlStep StepGotoUrl = new GotoUrlStep();
        private static readonly SelectStep StepSelect = new SelectStep();
        private static readonly ReadAttributeStep StepReadAttribute = new ReadAttributeStep();
        private static readonly ReadTextStep StepReadText = new ReadTextStep();
        private static readonly List<RecipeScraperBase> Scrappers = new List<RecipeScraperBase>{
            new IndianSimmer(),
            new AayisRecipes(),
        };
        #endregion

        public abstract List<ScrapedRecipe> ScrapeRecipies();

        protected CQ GotoUrl(string url)
        {
            return (CQ)StepGotoUrl.Process(url);
        }

        protected List<IDomElement> SelectItems(CQ cq, string css)
        {
            return (List<IDomElement>)StepSelect.Process(cq, css);
        }

        protected string ReadAttribute(IDomElement element, string attribute)
        {
            return StepReadAttribute.Process(element, attribute).ToString();
        }

        protected string ReadText(IDomElement element)
        {
            return StepReadText.Process(element).ToString();
        }

        public abstract string RootUrl { get;}

        public static List<ScrapedRecipe> ScrapeRecipies(User author)
        {
            var scraper = Scrappers.First(x => author.Sites.Any(y => y.ScrappingRootUrl == x.RootUrl) );
            return scraper.ScrapeRecipies();
        }

        #region Comment

        //private CQ MoveToUrl(ScraperLogic logic)
        //{
        //    return CQ.CreateFromUrl(logic.Url);
        //}

        //private List<IDomElement> SelectList(ScraperLogic logic, CQ cq)
        //{
        //    return cq.Select(logic.Css).Elements.ToList();
        //}

        //private string ReadData(ScraperLogic logic, IDomElement elem)
        //{
        //    if (logic.StepType == ScraperStepType.ReadAttribute)
        //        return elem.Attributes[logic.Css];
        //    else if (logic.StepType == ScraperStepType.ReadText)
        //        return elem.InnerText;
        //    throw new Exception("invalid step");

        //}

        //private ScraperLogic Process(ScraperLogic logic, CQ dom, List<IDomElement> elements, IDomElement element)
        //{
        //    while (logic.NextStep != null)
        //    {
        //        CQ newDom = null;
        //        List<IDomElement> newElements = null;
        //        switch (logic.StepType)
        //        {
        //            case ScraperStepType.MoveToUrl:
        //                newDom = MoveToUrl(logic);
        //                break;
        //            case ScraperStepType.SelectList:
        //                newElements = SelectList(logic, dom);
        //                break;
        //            case ScraperStepType.Loop:
        //                foreach (var elem in elements)
        //                {
        //                    Process(logic.NextStep, newDom, newElements, elem);
        //                }
        //                break;
        //            case ScraperStepType.ReadText:
        //            case ScraperStepType.ReadAttribute:
        //                ReadData(logic, element);
        //                break;
        //            case ScraperStepType.ReadUrl:
        //                logic.NextStep.Url = ReadData(logic, element);
        //                break;
        //        }
                
        //        return Process(logic.NextStep, newDom, newElements, null);
        //    }
        //    return null;
        //}#

        #endregion
    }

    public interface IScraperStep
    {
        object Process(params object[] args);
    }

    public class GotoUrlStep : IScraperStep
    {

        public object Process(params object[] args)
        {
            if (args == null || args.Length < 1)
                throw new ArgumentException("args");
            return CQ.CreateFromUrl(args[0].ToString());
        }
    }

    public class SelectStep : IScraperStep
    {
        public object Process(params object[] args)
        {
            if (args == null || args.Length < 2)
                throw new ArgumentException("args");
            return  ((CQ)args[0]).Select(args[1].ToString()).Elements.ToList();
        }
    }

    public class ReadAttributeStep : IScraperStep
    {
        public object Process(params object[] args)
        {

            if (args == null || args.Length < 2)
                throw new ArgumentException("args");
            return ((IDomElement)args[0]).Attributes[args[1].ToString()];
        }
    }

    public class ReadTextStep : IScraperStep
    {
        public object Process(params object[] args)
        {
            if (args == null || args.Length < 1)
                throw new ArgumentException("args");
            return ((IDomElement)args[0]).InnerText;
        }
    }




  
}
