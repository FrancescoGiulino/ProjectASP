% ------------------------------------------------------------------------------------------------------------------------------
% Sensing
task(Id,Sender,MessageState,X,Y,Z,TaskType) :-  aiMessage_ID(WIM,ObjectIndex,Index,Id),
                                                aiMessage_SenderName(WIM,ObjectIndex,Index,Sender),
                                                aiMessage_MessageState(WIM,ObjectIndex,Index,MessageState),
                                                aiMessage_X(WIM,ObjectIndex,Index,X),
                                                aiMessage_Y(WIM,ObjectIndex,Index,Y),
                                                aiMessage_Z(WIM,ObjectIndex,Index,Z),
                                                aiMessage_TaskType(WIM,ObjectIndex,Index,TaskType),
                                                MessageState != "Expired".

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


% ------------------------------------------------------------------------------------------------------------------------------
%  Rules
{ takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType) : task(TaskId,Sender,MessageState,X,Y,Z,TaskType), MessageState == "Pending" } <= 1.
:~ #count{TaskId: takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType)}=0, task(_,_,_,_,_,_,_). [1@1]

applyAction(1,"EnemyAction").
actionArgument(1,"MessageIndex",TaskId) :- takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType).
actionArgument(1,"EnemyName",Name) :- self(SelfId,Name,X,Y,Z,ReasoningStyle,CurrentStateName,CurrentHealth,IsDead).

#show task/7.
#show takeTask/7.
#show self/9.
%#show enemies/9.
#show applyAction/2.
#show actionArgument/3.