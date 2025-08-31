using UnityEngine;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;
namespace ThinkEngine
{
	public class lumenSentinelSensor_minBatteryBeforeRecharge : Sensor
	{
		private int counter;
		private object specificValue;
		private Operation operation;
		private BasicTypeMapper mapper;
		private List<float> values = new List<float>();
		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(float));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "lumenSentinelSensor_minBatteryBeforeRecharge(lumenSentinel,objectIndex("+index+"),{0})." + Environment.NewLine;
		}
		public override void Destroy()
		{
		}
		public override void Update()
		{
			if(!ready)
			{
				return;
			}
			if(!invariant || first)
			{
				first = false;
				if(gameObject == null)
				{
					values.Clear();
					return;
				}
				GameObject gameObject_1 = gameObject.gameObject;
				if(gameObject_1 == null)
				{
					values.Clear();
					return;
				}
				EnemyStateController EnemyStateController_2 = gameObject_1.GetComponent<EnemyStateController>();
				if(EnemyStateController_2 == null)
				{
					values.Clear();
					return;
				}
				if(EnemyStateController_2 == null)
				{
					values.Clear();
					return;
				}
				EnemyResources Resources_3 = EnemyStateController_2.Resources;
				if(Resources_3 == null)
				{
					values.Clear();
					return;
				}
				if(Resources_3 == null)
				{
					values.Clear();
					return;
				}
				float minBatteryBeforeRecharge_4 = Resources_3.minBatteryBeforeRecharge;
				if (values.Count == 1)
				{
					values.RemoveAt(0);
				}
				values.Add(minBatteryBeforeRecharge_4);
			}
		}
		public override string Map()
		{
			object operationResult = operation(values, specificValue, counter);
			if(operationResult != null)
			{
				return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
			}
			else
			{
				return "";
			}
		}
	}
}