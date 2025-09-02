% RICORDA: Per identificare i nemici nelle regole usa il nome, l'id a volte dà problemi!
% =================================================================================================================================
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

distanceSelfToTask(TaskId,Distance) :-
    self(_,Name,_,_,_,_,_,_,_),
    worldInformationManagerSensor_EnemyName(WIM, objectIndex(EnemyId), Index, Name),
    worldInformationManagerSensor_TaskId(WIM, objectIndex(EnemyId), Index, TaskId),
    worldInformationManagerSensor_Distance(WIM, objectIndex(EnemyId), Index, Distance).

% =================================================================================================================================
%  REGOLE:
nTasks(N) :- #count{Id: task(Id,_,MessageState,_,_,_,_,_,_), MessageState="Pending"} = N.
hasActiveTask :- task(_,_,_,_,_,_,_,Name,_), self(_,Name,_,_,_,_,_,_,_).

{takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType) :
    task(TaskId,Sender,MessageState,X,Y,Z,TaskType,AssignedTo,IsTaken),
    MessageState="Pending", AssignedTo="null", IsTaken=false,
    not hasActiveTask
} <= 1.

% =================================================================================================================================
% PENALIZZAZIONI:
% Penalizzazione se non si prende nessun task quando ce n'è almeno uno (del tipo ottimo) disponibile:
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"investigation")}=0,
    self(_,Name,"EcoSentinel",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"investigation",_,_). [1@3]

:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"reinforcement")}=0,
    self(_,Name,"OverrideStalker",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"reinforcement",_,_). [1@3]

% Penalizzazione per distanza:
    % se si prende un task che dista più di 10 unità, allora penitenza proporzionale alla distanza
    :~ takeTask(TaskId,_,_,_,_,_,_), distanceSelfToTask(TaskId,Distance), Distance>10. [Distance@2,TaskId,Distance]
% =================================================================================================================================
% ACTIONS:
    applyAction(1,"TakeTask") :- takeTask(_,_,_,_,_,_,_).
    actionArgument(1,"MessageIndex",TaskId) :- takeTask(TaskId,_,_,_,_,_,_).
    actionArgument(1,"EnemyName",Name) :- takeTask(_,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_).

%#show self/9.
%#show enemies/9.
#show task/9.
#show takeTask/7.
#show hasActiveTask/0.
#show distanceSelfToTask/2.
#show applyAction/2.
#show actionArgument/3.