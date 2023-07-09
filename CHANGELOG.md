0.3.0 - 7/8/2023

- Added support for DE6 and DH4.
- Each locomotive's cruise control settings can be tuned.
- Cleaned up the stats a bit. They can be disabled.
- Fixed a bug with braking prediction. Braking overshoots less now.
- Braking prediction time is now configurable with the `DecelerationTime` setting.
- Locomotives now brake when torque decreases instead of amps (DH4 has no amps)
- Removed `OverdriveEnabled` setting. If you don't want to use overdrive set `OverdriveTemperature` to the same as `MaxTemperature`.

0.2.0 - 7/7/2023

- Fixed some crashes with non-DE2 locos

0.1.0 - 7/7/2023

- Initial Release