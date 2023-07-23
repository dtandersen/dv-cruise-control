using System;
using Xunit;
using Xunit.Abstractions;

namespace DriverAssist.Cruise
{
    [Collection("Sequential")]
    public class ShiftSystemTest
    {
        private LocoController loco;
        private FakeTrainCarWrapper train;
        private ShiftSystem system;

        public ShiftSystemTest(ITestOutputHelper output)
        {
            PluginLoggerSingleton.Instance = new TestLogger(output);

            train = new FakeTrainCarWrapper
            {
                LocoType = DriverAssist.LocoType.DE2,
                Reverser = 1
            };

            loco = new LocoController(1f / 60f);
            loco.UpdateLocomotive(train);
            system = new ShiftSystem(loco);
        }

        public void Dispose()
        {
            PluginLoggerSingleton.Instance = new NullLogger();
        }

        /// The train is a DM3
        /// and a gear change has been requested.
        /// Proceed.
        [Fact]
        public void ShiftGearsInDm3()
        {
            train.LocoType = LocoType.DM3;
            train.Throttle = 1;
            train.GearboxA = 0;
            train.GearboxB = 0;

            train.GearChangeInProgress = false;
            loco.ChangeGear(3);
            WhenSystemUpdates();
            Assert.Equal(0, train.GearboxA);
            Assert.Equal(0, train.GearboxB);
            Assert.Equal(0, train.Throttle);

            WhenSystemUpdates();
            Assert.Equal(0.5f, train.GearboxA);
            Assert.Equal(0.5f, train.GearboxB);
            Assert.Equal(0, train.Throttle);

            train.GearChangeInProgress = true;
            WhenSystemUpdates();
            Assert.Equal(0, train.Throttle);

            train.GearChangeInProgress = false;
            WhenSystemUpdates();
            Assert.Equal(1, train.Throttle);
            Assert.Null(loco.Components.GearChangeRequest);
        }

        /// The train is a DM3
        /// and Throttle is 0.
        /// and a gear change has been requested.
        /// The gear change should be instant.
        [Fact]
        public void ChangeGearInstantlyIfZeroThrottle()
        {
            train.LocoType = LocoType.DM3;
            train.Throttle = 0;
            train.GearboxA = 0;
            train.GearboxB = 0;

            loco.ChangeGear(3);
            WhenSystemUpdates();
            Assert.Equal(0.5f, train.GearboxA);
            Assert.Equal(0.5f, train.GearboxB);
            Assert.Equal(0, train.Throttle);
            Assert.Null(loco.Components.GearChangeRequest);
        }

        /// The train is a DM3
        /// and Throttle is 1
        /// and no gear change has been requested.
        /// Leave Throttle at 1.
        [Fact]
        public void DoesntAdjustThrottleIfNotShifting()
        {
            train.LocoType = LocoType.DM3;
            train.Throttle = 1;
            train.GearboxA = 0;
            train.GearboxB = 0;

            train.GearChangeInProgress = false;
            loco.Gear = 0;
            loco.ChangeGear(0);

            WhenSystemUpdates();
            Assert.Equal(0, train.GearboxA);
            Assert.Equal(0, train.GearboxB);
            Assert.Equal(1, train.Throttle);
        }

        /// The train is a DM3
        /// and Throttle is 1
        /// and no gear change has been requested.
        /// Leave Throttle at 1.
        [Fact]
        public void DoesntRestoreThrottleIfNotRequested()
        {
            train.LocoType = LocoType.DM3;
            train.Throttle = 0;
            // train.GearboxA = 0;
            // train.GearboxB = 0;

            // train.GearChangeInProgress = false;
            // loco.Gear = 0;
            loco.ChangeGear(1);
            train.Throttle = 1;
            Assert.Null(loco.Components.GearChangeRequest.Value.RestoreThrottle);

            WhenSystemUpdates();
            Assert.NotNull(loco.Components.GearChangeRequest);
            Assert.Null(loco.Components.GearChangeRequest.Value.RestoreThrottle);
            Assert.Equal(0, train.Throttle);

            WhenSystemUpdates();
            Assert.Null(loco.Components.GearChangeRequest);
            Assert.Equal(0, train.Throttle);
        }

        /// The train is a DE2
        /// and a gear change has been requested.
        /// Ignore it.
        [Fact]
        public void DoesntAutoShiftIfTrainNotMechanical()
        {
            train.Throttle = 1;
            train.GearboxA = 0;
            train.GearboxB = 0;

            loco.ChangeGear(2);
            WhenSystemUpdates();

            Assert.Equal(0, train.GearboxA);
            Assert.Equal(0, train.GearboxB);
            Assert.Equal(1, train.Throttle);
        }

        private void WhenSystemUpdates()
        {
            system.OnUpdate();
        }
    }
}