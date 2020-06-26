using Common.Standard.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.Common.Standard
{
    public class BuilderTests
    {
        [Fact]
        public void TestGoFBuilder()
        {
            PizzaBuilder pb = new PizzaBuilder();
            var myPizza = pb.Build("thin", "normal", "onion", "mushroom", "black olives");

            Assert.True(myPizza.IsReady);
        }

        [Fact]
        public void TestGoFBuilderForNullArgs()
        {
            try
            {
                PizzaBuilder pb = new PizzaBuilder();
                var myPizza = pb.Build(null);

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void TestGoFBuilderForBadArgs()
        {
            try
            {
                PizzaBuilder pb = new PizzaBuilder();
                var myPizza = pb.Build("thin", "normal", "anchovies", "mushroom", "black olives");

                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException);
            }
        }

    }

    #region Extending classes
    class PizzaBuilder : AbstractBuilder<Pizza>
    {

        public PizzaBuilder()
        {
            AddDelegate(SetCrust);
            AddDelegate(AddSauce);
            AddDelegate(AddToppings);
            AddDelegate(DetermineIfReady);
        }

        private bool SetCrust(Pizza pizza, params object[] args)
        {
            pizza.Crust = args[0].ToString();
            return true;
        }

        private bool AddSauce(Pizza pizza, params object[] args)
        {
            pizza.Sauce = args[1].ToString();
            return !string.IsNullOrEmpty(args.ToString());
        }

        private bool AddToppings(Pizza pizza, params object[] args)
        {
            for(var i = 2; i < args.Length; i++)
            {
                if (args[i].ToString() == "anchovies")
                {
                    return false;
                }

                pizza.Toppings.Add(args[i].ToString());
            }

            return pizza.Toppings.Count > 0;
        }

        private bool DetermineIfReady(Pizza pizza, params object[] args)
        {
            pizza.IsReady = true;

            return pizza.IsReady;
        }
            
    }
    #endregion

    #region DataClasses
    class Pizza
    {
        public string Crust { get; set; }

        public string Sauce { get; set; }

        public List<string> Toppings { get; set; }

        public bool IsReady { get; set; }

        public Pizza()
        {
            Toppings = new List<string>();
        }
    }
    #endregion
}
