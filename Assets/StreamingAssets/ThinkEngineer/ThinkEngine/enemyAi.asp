% ------------------------------------------------------------------------------------------------------------------------------
% RICORDA: Per identificare i nemici nelle regole usa il nome, l'id a volte dà problemi!

% Sensing
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

% distanza dai task rilevanti (Pending e non "information")
%distanceEnemyToTask(EnemyName,TaskId,Distance) :-
%    worldInformationManagerSensor_EnemyName(WIM, objectIndex(EnemyId), Index, EnemyName),
%    worldInformationManagerSensor_TaskId(WIM, objectIndex(EnemyId), Index, TaskId),
%    worldInformationManagerSensor_Distance(WIM, objectIndex(EnemyId), Index, Distance).

distanceSelfToTask(TaskId,Distance) :-
    self(_,Name,_,_,_,_,_,_,_),
    worldInformationManagerSensor_EnemyName(WIM, objectIndex(EnemyId), Index, Name),
    worldInformationManagerSensor_TaskId(WIM, objectIndex(EnemyId), Index, TaskId),
    worldInformationManagerSensor_Distance(WIM, objectIndex(EnemyId), Index, Distance).

% ------------------------------------------------------------------------------------------------------------------------------
%  Rules
nTasks(N) :- #count{Id: task(Id,_,MessageState,_,_,_,_,_,_), MessageState="Pending"} = N.
hasTask :- task(_,_,_,_,_,_,_,Name,_), self(_,Name,_,_,_,_,_,_,_).

{takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType) :
    task(TaskId,Sender,MessageState,X,Y,Z,TaskType,AssignedTo,IsTaken),
    MessageState="Pending", AssignedTo="null", IsTaken=false,
    not hasTask
} <= 1.
:~ #count{TaskId: takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType)}=0, not hasTask. [1@1]

% se una EcoSentinel prende un task "reinforcement", allora penitenza.
:~ self(_,_,"EcoSentinel",_,_,_,_,_,false), takeTask(_,_,_,_,_,_,"reinforcement"). [1@2]
% se un OverrideStalker prende un task di "investigation"
:~ self(_,_,"OverrideStalker",_,_,_,_,_,false), takeTask(_,_,_,_,_,_,"investigation"). [1@2]
% se ci sono sia task di "investigation" che di "reinforcement" ed un CloseRangeEnforcer sceglie di prendere un task di "investigation", allora penitenza.
:~ self(_,_,"CloseRangeEnforcer",_,_,_,_,_,false), takeTask(_,_,_,_,_,_,"investigation"), task(_,_,_,"Pending",_,"reinforcement",_). [1@2]

applyAction(1,"EnemyAction") :- takeTask(_,_,_,_,_,_,_).
actionArgument(1,"MessageIndex",TaskId) :- takeTask(TaskId,_,_,_,_,_,_).
actionArgument(1,"EnemyName",Name) :- takeTask(_,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_).

#show task/9.
#show takeTask/7.
#show hasTask/0.
%#show self/9.
%#show enemies/9.
%#show distanceSelfToTask/2.
#show applyAction/2.
#show actionArgument/3.