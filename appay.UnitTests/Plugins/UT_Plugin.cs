using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Plugins;
using System;

namespace Neo.UnitTests.Plugins
{
    [TestClass]
    public class UT_Plugin
    {
        private static readonly object locker = new object();

        [TestMethod]
        public void TestGetConfigFile()
        {
            var pp = new TestLogPlugin();
            var file = pp.ConfigFile;
            file.EndsWith("config.json").Should().BeTrue();
        }

        [TestMethod]
        public void TestGetName()
        {
            var pp = new TestLogPlugin();
            pp.Name.Should().Be("TestLogPlugin");
        }

        [TestMethod]
        public void TestGetVersion()
        {
            var pp = new TestLogPlugin();
            Action action = () => pp.Version.ToString();
            action.Should().NotThrow();
        }

        [TestMethod]
        public void TestLog()
        {
            var lp = new TestLogPlugin();
            lp.LogMessage("Hello");
            lp.Output.Should().Be("Plugin:TestLogPlugin_Info_Hello");
        }

        [TestMethod]
        public void TestSendMessage()
        {
            lock (locker)
            {
                Plugin.Plugins.Clear();
                Plugin.SendMessage("hey1").Should().BeFalse();

                var lp = new TestLogPlugin();
                Plugin.SendMessage("hey2").Should().BeTrue();
            }
        }

        [TestMethod]
        public void TestNotifyPluginsLoadedAfterSystemConstructed()
        {
            var pp = new TestLogPlugin();
            Action action = () => Plugin.NotifyPluginsLoadedAfterSystemConstructed();
            action.Should().NotThrow();
        }

        [TestMethod]
        public void TestResumeNodeStartupAndSuspendNodeStartup()
        {
            var system = TestBlockchain.InitializeMockNeoSystem();
            TestLogPlugin.TestLoadPlugins(system);
            TestLogPlugin.TestSuspendNodeStartup();
            TestLogPlugin.TestSuspendNodeStartup();
            TestLogPlugin.TestResumeNodeStartup().Should().BeFalse();
            TestLogPlugin.TestResumeNodeStartup().Should().BeTrue();
        }

        [TestMethod]
        public void TestGetConfiguration()
        {
            var pp = new TestLogPlugin();
            pp.TestGetConfiguration().Key.Should().Be("PluginConfiguration");
        }
    }
}
