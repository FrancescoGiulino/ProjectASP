task(Sender,MessageState,X,Y,Z,TaskType) :- aiMessage_SenderName(WIM,ObjectIndex,Index,Sender),
                                            aiMessage_MessageState(WIM,ObjectIndex,Index,MessageState),
                                            aiMessage_X(WIM,ObjectIndex,Index,X),
                                            aiMessage_Y(WIM,ObjectIndex,Index,Y),
                                            aiMessage_Z(WIM,ObjectIndex,Index,Z),
                                            aiMessage_TaskType(WIM,ObjectIndex,Index,TaskType),
                                            MessageState != "Expired".

enemies(SelfId,Name,X,Y,Z,CurrentStateName,CurrentHealth,IsDead) :-
    lumenSentinelSensor_name(EcoSentinel,SelfId,Name),
    lumenSentinelSensor_X(EcoSentinel,SelfId,X),
    lumenSentinelSensor_Y(EcoSentinel,SelfId,Y),
    lumenSentinelSensor_Z(EcoSentinel,SelfId,Z),
    lumenSentinelSensor_currentStateName(EcoSentinel,SelfId,CurrentStateName),
    lumenSentinelSensor_CurrentHealth(EcoSentinel,SelfId,CurrentHealth),
    lumenSentinelSensor_IsDead(EcoSentinel,SelfId,IsDead).

#show task/6.
#show enemies/8.

% applyAction(1,"EnemyAction").
% actionArgument(1,"MessageIndex",0).
% actionArgument(1,"EnemyName","CloseRangeEnforcer (0)").