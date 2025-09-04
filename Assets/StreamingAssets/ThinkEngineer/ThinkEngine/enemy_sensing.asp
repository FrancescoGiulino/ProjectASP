% SENSING:
task(Id,Sender,MessageState,X,Y,Z,TaskType,AssignedTo,IsTaken) :-   aiMessage_ID(WIM,ObjectIndex,Index,Id),
                                                                    aiMessage_SenderName(WIM,ObjectIndex,Index,Sender),
                                                                    aiMessage_MessageState(WIM,ObjectIndex,Index,MessageState),
                                                                    aiMessage_X(WIM,ObjectIndex,Index,X),
                                                                    aiMessage_Y(WIM,ObjectIndex,Index,Y),
                                                                    aiMessage_Z(WIM,ObjectIndex,Index,Z),
                                                                    aiMessage_TaskType(WIM,ObjectIndex,Index,TaskType),
                                                                    aiMessage_AssignedTo(WIM,ObjectIndex,Index,AssignedTo),
                                                                    aiMessage_IsTaken(WIM,ObjectIndex,Index,IsTaken).
                                                                    %MessageState != "Expired".

enemies(SelfId,Name,ReasoningStyle,X,Y,Z,CurrentStateName,CurrentHealth,IsDead) :-
    lumenSentinelSensor_name(EcoSentinel,SelfId,Name),
    lumenSentinelSensor_ReasoningStyleType(EcoSentinel,SelfId,ReasoningStyle),
    lumenSentinelSensor_X(EcoSentinel,SelfId,X),
    lumenSentinelSensor_Y(EcoSentinel,SelfId,Y),
    lumenSentinelSensor_Z(EcoSentinel,SelfId,Z),
    lumenSentinelSensor_currentStateName(EcoSentinel,SelfId,CurrentStateName),
    lumenSentinelSensor_CurrentHealth(EcoSentinel,SelfId,CurrentHealth),
    lumenSentinelSensor_IsDead(EcoSentinel,SelfId,IsDead).

self(SelfId, Name, ReasoningStyle, X, Y, Z, CurrentStateName, CurrentHealth, IsDead) :-
    currentBrainID(SelfId),
    enemies(objectIndex(SelfId), Name, ReasoningStyle, X, Y, Z, CurrentStateName, CurrentHealth, IsDead).

enemyMinBattery(EnemyName,MinBattery):-
    enemies(ID,EnemyName,RS,X,Y,Z,CurrentStateName,Battery,IsDead),
    lumenSentinelSensor_minBatteryBeforeRecharge(WIM,ID,MinBattery).

selfMinBattery(MinBattery) :- self(_,SelfName,_,_,_,_,_,_,_), enemyMinBattery(SelfName,MinBattery).

distanceEnemyToTask(Name,TaskId,Distance) :-
    worldInformationManagerSensor_EnemyName(WIM, objectIndex(EnemyId), Index, Name),
    worldInformationManagerSensor_TaskId(WIM, objectIndex(EnemyId), Index, TaskId),
    worldInformationManagerSensor_Distance(WIM, objectIndex(EnemyId), Index, Distance).

distanceSelfToTask(TaskId,Distance) :-
    self(_,Name,_,_,_,_,_,_,_),
    distanceEnemyToTask(Name,TaskId,Distance).
