using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GoogleIABManager : AbstractManager
{
	public static bool _bBillingSupport;

	public static event Action billingSupportedEvent;

	public static event Action<string> billingNotSupportedEvent;

	public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent;

	public static event Action<string> queryInventoryFailedEvent;

	public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent;

	public static event Action<GooglePurchase> purchaseSucceededEvent;

	public static event Action<string> purchaseFailedEvent;

	public static event Action<GooglePurchase> consumePurchaseSucceededEvent;

	public static event Action<string> consumePurchaseFailedEvent;

	static GoogleIABManager()
	{
//		AbstractManager.initialize(typeof(GoogleIABManager));
	}

	public void billingSupported(string empty)
	{
		Debug.Log("billingSupported:" + empty);
		GoogleIABManager.billingSupportedEvent.fire();
	}

	public void billingNotSupported(string error)
	{
		Debug.Log("billingNotSupported:" + error);
		GoogleIABManager.billingNotSupportedEvent.fire(error);
		_bBillingSupport = false;
	}

	public void queryInventorySucceeded(string json)
	{
		Debug.Log("queryInventorySucceeded:" + json);
		_bBillingSupport = true;
		if (GoogleIABManager.queryInventorySucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			GoogleIABManager.queryInventorySucceededEvent(GooglePurchase.fromList(dictionary["purchases"] as List<object>), GoogleSkuInfo.fromList(dictionary["skus"] as List<object>));
		}
	}

	public void queryInventoryFailed(string error)
	{
		Debug.Log("queryInventoryFailed:" + error);
		GoogleIABManager.queryInventoryFailedEvent.fire(error);
		_bBillingSupport = false;
	}

	public void purchaseCompleteAwaitingVerification(string json)
	{
		Debug.Log("purchaseCompleteAwaitingVerification:" + json);
		if (GoogleIABManager.purchaseCompleteAwaitingVerificationEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = dictionary["purchaseData"].ToString();
			string arg2 = dictionary["signature"].ToString();
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent(arg, arg2);
		}
	}

	public void purchaseSucceeded(string json)
	{
		Debug.Log("purchaseSucceeded:" + json);
		GoogleIABManager.purchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
	}

	public void purchaseFailed(string error)
	{
		Debug.Log("purchaseFailed:" + error);
		GoogleIABManager.purchaseFailedEvent.fire(error);
	}

	public void consumePurchaseSucceeded(string json)
	{
		Debug.Log("consumePurchaseSucceeded:" + json);
		if (GoogleIABManager.consumePurchaseSucceededEvent != null)
		{
			GoogleIABManager.consumePurchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
		}
	}

	public void consumePurchaseFailed(string error)
	{
		Debug.Log("consumePurchaseFailed:" + error);
		GoogleIABManager.consumePurchaseFailedEvent.fire(error);
	}
}
