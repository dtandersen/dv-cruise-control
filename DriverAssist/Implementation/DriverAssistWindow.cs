using System;
using DriverAssist.Cruise;
using DriverAssist.Localization;
using UnityEngine;

namespace DriverAssist.Implementation
{

    class DriverAssistWindow : MonoBehaviour
    {
        public LocoController loco { get; internal set; }
        public CruiseControl cruiseControl { get; internal set; }
        public UnifiedSettings config { get; internal set; }

        private Rect windowRect;
        private bool loaded;
        private Translation localization;
        private float scale = 1.5f;

        void Awake()
        {
            PluginLoggerSingleton.Instance.Info("DriverAssistWindow::Awake");
            localization = TranslationManager.Current;
            windowRect = new Rect(20, 20, scale * 120, scale * 50);
        }

        void OnGUI()
        {
            // PluginLoggerSingleton.Instance.Info($"loco={loco} loaded={loaded}");
            if (loco == null) return;
            // PluginLoggerSingleton.Instance.Info($"IsLoco={loco.IsLoco}");
            if (!loaded) return;

            if (Event.current.keyCode == KeyCode.Tab || Event.current.character == '\t')
                Event.current.Use();

            if (!loco.IsLoco) return;

            GUI.skin = DVGUI.skin;

            windowRect = GUILayout.Window(4444324, windowRect, GUIWindow, "stats");
        }

        private void GUIWindow(int id)
        {
            Window();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        protected void Window()
        {
            float Speed = loco.RelativeSpeedKmh;
            float Throttle = loco.Throttle;
            float Mass = loco.Mass;
            float powerkw = Mass * loco.RelativeAccelerationMs * loco.RelativeSpeedKmh / 3.6f / 1000;
            float Force = Mass * 9.8f / 2f;

            int labelwidth = (int)(scale * 100);
            int width = (int)(scale * 50);

            GUILayout.BeginHorizontal();
            GUILayout.Label(localization.CC_SETPOINT, GUILayout.Width(labelwidth));
            GUILayout.TextField($"{cruiseControl.DesiredSpeed}", GUILayout.Width(width));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(localization.CC_STATUS, GUILayout.Width(labelwidth));
            GUILayout.TextField($"{cruiseControl.Status}", GUILayout.Width(scale * 150));
            GUILayout.EndHorizontal();

            if (config.ShowStats)
            {
                int mass = (int)(Mass / 1000);
                int locoMass = (int)(loco.LocoMass / 1000);
                int cargoMass = (int)(loco.CargoMass / 1000);

                GUILayout.BeginHorizontal();
                GUILayout.Label($"", GUILayout.Width(labelwidth));
                GUILayout.Label(localization.TRAIN, GUILayout.Width(width));
                GUILayout.Label(localization.LOCO_ABBV, GUILayout.Width(width));
                GUILayout.Label(localization.CARGO, GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label($"{localization.STAT_MASS}", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{mass}", GUILayout.Width(width));
                GUILayout.TextField($"{locoMass}", GUILayout.Width(width));
                GUILayout.TextField($"{cargoMass}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                float predTime = 5f;

                GUILayout.BeginHorizontal();
                GUILayout.Label($"", GUILayout.Width(labelwidth));
                GUILayout.Label(localization.STAT_CURRENT, GUILayout.Width(width));
                GUILayout.Label($"{localization.STAT_CHANGE}/s", GUILayout.Width(width));
                GUILayout.Label($"{(int)predTime}s", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_SPEED, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.RelativeSpeedKmh.ToString("N1")}", GUILayout.Width(width));
                GUILayout.TextField($"{loco.RelativeAccelerationMs.ToString("N3")}", GUILayout.Width(width));
                GUILayout.TextField($"{(loco.RelativeSpeedKmh + predTime * loco.RelativeAccelerationMs * 3.6f).ToString("N1")}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_TEMPERATURE, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.Temperature.ToString("N1")}", GUILayout.Width(width));
                GUILayout.TextField($"{loco.TemperatureChange.ToString("N2")}", GUILayout.Width(width));
                GUILayout.TextField($"{(loco.Temperature + predTime * loco.TemperatureChange).ToString("N1")}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_AMPS, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.Amps.ToString("N0")}", GUILayout.Width(width));
                GUILayout.TextField($"{loco.AmpsRoc.ToString("N1")}", GUILayout.Width(width));
                GUILayout.TextField($"{(loco.Amps + predTime * loco.AmpsRoc).ToString("N0")}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_RPM, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{Math.Round(loco.Rpm, 0)}", GUILayout.Width(width));
                GUILayout.TextField($"", GUILayout.Width(width));
                GUILayout.TextField($"", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_TORQUE, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{(int)loco.RelativeTorque}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(localization.STAT_THROTTLE, GUILayout.Width(labelwidth));
                GUILayout.TextField($"{(int)(loco.Throttle * 100)}%", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Gear", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.Gear + 1}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                // if (loco.Components.GearChangeRequest.HasValue)
                // {
                GearChangeRequest? req = loco.Components?.GearChangeRequest;
                GUILayout.BeginHorizontal();
                GUILayout.Label("Requested Gear", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{req?.RequestedGear ?? -1}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Restore Throttle", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{req?.RestoreThrottle ?? -1}", GUILayout.Width(width));
                GUILayout.EndHorizontal();
                // }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Gear Ratio", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.GearRatio}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Shifting", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.GearShiftInProgress}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Train Brake", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{loco.TrainBrake.ToString("F2")}", GUILayout.Width(width));
                GUILayout.EndHorizontal();

                float speed2 = 3f / 25f * (float)Math.PI * loco.WheelRadius * loco.Rpm / loco.GearRatio;
                GUILayout.BeginHorizontal();
                GUILayout.Label("Speed", GUILayout.Width(labelwidth));
                GUILayout.TextField($"{speed2.ToString("N1")}", GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
        }

        void OnDestroy()
        {
            PluginLoggerSingleton.Instance.Info("DriverAssistWindow::OnDestroy");
        }

        public void OnLoad(object sender, EventArgs e)
        {
            PluginLoggerSingleton.Instance.Info("DriverAssistWindow::OnLoad");
            loaded = true;
        }

        public void OnUnload(object sender, EventArgs e)
        {
            PluginLoggerSingleton.Instance.Info("DriverAssistWindow::OnUnload");
            loaded = false;
        }
    }
}