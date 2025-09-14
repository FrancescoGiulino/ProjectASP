using UnityEngine;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;
namespace ThinkEngine
{
	public class lumenSentinelSensor_HealthController_IsDead : Sensor
	{
		private int counter;
		private object specificValue;
		private Operation operation;
		private BasicTypeMapper mapper;
		private List<bool> values = new List<bool>();
		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(bool));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "lumenSentinelSensor_HealthController_IsDead(ecoSentinel7,objectIndex("+index+"),{0})." + Environment.NewLine;
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
				HealthController HealthController_2 = EnemyStateController_1.HealthController;
				if(HealthController_2 == null)
				{
					values.Clear();
					return;
				}
				if(HealthController_2 == null)
				{
					values.Clear();
					return;
				}
				bool IsDead_3 = HealthController_2.IsDead;
				if (values.Count == 1)
				{
					values.RemoveAt(0);
				}
				values.Add(IsDead_3);
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