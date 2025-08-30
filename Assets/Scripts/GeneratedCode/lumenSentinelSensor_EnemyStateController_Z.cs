using UnityEngine;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;
namespace ThinkEngine
{
	public class lumenSentinelSensor_EnemyStateController_Z : Sensor
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
			mappingTemplate = "lumenSentinelSensor_EnemyStateController_Z(closeRangeEnforcer0,objectIndex("+index+"),{0})." + Environment.NewLine;
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
				EnemyStateController EnemyStateController_1 = gameObject.GetComponent<EnemyStateController>();
				if(EnemyStateController_1 == null)
				{
					values.Clear();
					return;
				}
				if(EnemyStateController_1 == null)
				{
					values.Clear();
					return;
				}
				int Z_2 = EnemyStateController_1.Z;
				if (values.Count == 1)
				{
					values.RemoveAt(0);
				}
				values.Add(Z_2);
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