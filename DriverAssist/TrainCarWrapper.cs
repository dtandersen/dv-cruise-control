using System.Collections.Generic;

namespace DriverAssist
{
#pragma warning disable IDE1006
    public interface TrainCarWrapper
#pragma warning restore IDE1006
    {
        float SpeedKmh { get; }
        float SpeedMs { get; }
        float Throttle { get; set; }
        float TrainBrake { get; set; }
        float IndBrake { get; set; }
        float GearboxA { get; set; }
        float GearboxB { get; set; }
        float Temperature { get; }
        float Reverser { get; set; }
        float Torque { get; }
        string TractionMotors { get; }
        float Amps { get; }
        float Rpm { get; }
        bool IsWheelSlipping { get; }
        float WheelSlip { get; }
        string LocoType { get; }
        float Mass { get; }
        float LocoMass { get; }
        float CargoMass { get; }
        float WheelRadius { get; }
        float GearRatio { get; }
        List<string> Ports { get; }
        bool GearChangeInProgress { get; }

        bool IsLoco { get; }
        int Length { get; }
    }

    public class NullTrainCarWrapper : TrainCarWrapper
    {
        private static readonly NullTrainCarWrapper instance;

        static NullTrainCarWrapper()
        {
            instance = new NullTrainCarWrapper();
        }

        public static TrainCarWrapper Instance
        {
            get
            {
                return instance;
            }
        }

        public bool IsLoco { get { return false; } }

        public float SpeedKmh { get; }

        public float SpeedMs { get; }

        public float Throttle { get; set; }
        public float TrainBrake { get; set; }
        public float IndBrake { get; set; }
        public float GearboxA { get; set; }
        public float GearboxB { get; set; }

        public float Temperature { get; }

        public float Reverser { get; set; }

        public float Torque { get; }

        public string TractionMotors { get; }

        public float Amps { get; }

        public float Rpm { get; }
        public float WheelSlip { get; }
        public bool IsWheelSlipping { get; }

        public string LocoType { get; }

        public float Mass { get; }
        public float LocoMass { get; }
        public float CargoMass { get; }

        public float WheelRadius { get; }
        public float GearRatio { get; }
        public List<string> Ports { get; }
        public bool GearChangeInProgress { get; }

        public int Length { get; }

        NullTrainCarWrapper()
        {
            Ports = new();
            TractionMotors = "";
            LocoType = DriverAssist.LocoType.DE2;
        }
    }

    public class FakeTrainCarWrapper : TrainCarWrapper
    {
        public string LocoType { get; set; }
        public bool IsLoco { get { return LocoType != ""; } }

        public float SpeedKmh { get; set; }

        public float SpeedMs { get; set; }

        public float Throttle { get; set; }
        public float TrainBrake { get; set; }
        public float IndBrake { get; set; }
        public float GearboxA { get; set; }
        public float GearboxB { get; set; }

        public float Temperature { get; set; }

        public float Reverser { get; set; }

        public float Torque { get; set; }

        public string TractionMotors { get; }

        public float Amps { get; set; }

        public float Rpm { get; set; }
        public float WheelSlip { get; }
        public bool IsWheelSlipping { get; }

        public float Mass { get; }
        public float LocoMass { get; }
        public float CargoMass { get; }
        public float WheelRadius { get; }
        public float GearRatio { get; }
        public List<string> Ports { get; }
        public bool GearChangeInProgress { get; set; }
        public int Length { get; set; }

        public FakeTrainCarWrapper()
        {
            Ports = new();
            TractionMotors = "";
            LocoType = DriverAssist.LocoType.DE2;
        }
    }
}
