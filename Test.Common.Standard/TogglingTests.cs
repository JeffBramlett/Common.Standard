using System;
using System.Collections.Generic;
using System.Text;
using Common.Standard.Toggles;
using Xunit;

namespace Test.Common.Standard
{
    public class TogglingTests
    {
        [Fact]
        public void Toggling_SimpleTest()
        {
            ToggleProvider.Instance.AddToggle("T1", DateTime.Now - TimeSpan.FromSeconds(1));

            var isToggled = ToggleProvider.Instance.IsToggled("T1");

            Assert.True(isToggled);
        }
    }
}
