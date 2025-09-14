using UnityEngine;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;
namespace ThinkEngine
{
	public class lumenSentinelSensor_Y : Sensor
	{
		private int counter;
		private object specificValue;
		private Operation operation;
		private BasicTypeMapper mapper;
		private List<int> values = new List<int>();
		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(int));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "lumenSentinelSensor_Y(lumenSentinel,objectIndex("+index+"),{0})." + Environment.NewLine;
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
				int Y_3 = EnemyStateController_2.Y;
				if (values.Count == 1)
				{
					values.RemoveAt(0);
				}
				values.Add(Y_3);
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