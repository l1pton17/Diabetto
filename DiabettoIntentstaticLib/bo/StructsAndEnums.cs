using System;
using ObjCRuntime;

namespace Diabetto
{
	[Watch (5,0), NoTV, NoMac, iOS (12,0)]
	[Native]
	public enum AddMeasureIntentResponseCode : nint
	{
		Unspecified = 0,
		Ready,
		ContinueInApp,
		InProgress,
		Success,
		Failure,
		FailureRequiringAppLaunch
	}

	[Watch (6,0), iOS (13,0)]
	[Native]
	public enum AddMeasureLevelUnsupportedReason : nint
	{
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}

	[Watch (6,0), iOS (13,0)]
	[Native]
	public enum AddMeasureBreadunitsUnsupportedReason : nint
	{
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}

	[Watch (6,0), iOS (13,0)]
	[Native]
	public enum AddMeasureShortinsulinUnsupportedReason : nint
	{
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}
}
