using System;
using ObjCRuntime;

namespace Diabetto
{
	[Watch (5,0), NoTV, NoMac, iOS (12,0)]
	[Native]
	public enum AddMeasureIntentResponseCode : long
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
	public enum AddMeasureLevelUnsupportedReason : long
    {
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}

	[Watch (6,0), iOS (13,0)]
	[Native]
	public enum AddMeasureBreadunitsUnsupportedReason : long
    {
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}

	[Watch (6,0), iOS (13,0)]
	[Native]
	public enum AddMeasureShortinsulinUnsupportedReason : long
    {
		NegativeNumbersNotSupported = 1,
		GreaterThanMaximumValue,
		LessThanMinimumValue
	}
}
