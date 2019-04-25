using System;
using System.Collections.Generic;
using System.Text;
using Common.Standard.Toggles;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Test.Common.Standard
{
    public class TogglingTests
    {
        [Fact]
        public void Toggling_SimpleTest()
        {
            string content = TogglesAsJsonContent();
            DefaultToggleRepository repo = new DefaultToggleRepository();
            repo.LoadFromJsonContent(content);
            repo.Init();

            ToggleProvider provider = new ToggleProvider(repo);

            Assert.True(provider.IsToggled("T1"));
        }

        [Fact]
        public void Toggling_Mock_Test()
        {
            string toggleKey = "unit test";

            Mock<IToggleRepository> repoMock = new Mock<IToggleRepository>();
            repoMock.Setup(r => r.HasToggleByKey(toggleKey)).Returns(true);
            repoMock.Setup(r => r.ToggleForKey(toggleKey)).Returns(new Toggle() { IsEnabled = true, Key = toggleKey, Start = DateTime.Now });

            ToggleProvider provider = new ToggleProvider(repoMock.Object);

            var isToggled = provider.IsToggled(toggleKey);

            Assert.True(isToggled);
        }

        private string TogglesAsJsonContent()
        {
            List<Toggle> toggleList = new List<Toggle>()
            {
                new Toggle()
                {
                    Key = "T1",
                    IsEnabled = true,
                    Start = DateTime.Now - TimeSpan.FromSeconds(5)
                },
                new Toggle()
                {
                    Key = "T2",
                    IsEnabled = true,
                    Start = DateTime.Now - TimeSpan.FromSeconds(5)
                }

            };

            return JsonConvert.SerializeObject(toggleList);
        }
    }
}
