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
% Regole basate sulla distanza:

%distanceEnemyToTask(Name,TaskId,Distance) :-
%    worldInformationManagerSensor_EnemyName(WIM, objectIndex(EnemyId), Index, Name),
%    worldInformationManagerSensor_TaskId(WIM, objectIndex(EnemyId), Index, TaskId),
%    worldInformationManagerSensor_Distance(WIM, objectIndex(EnemyId), Index, Distance).

%distanceSelfToTask(TaskId,Distance) :- self(_,Name,_,_,_,_,_,_,_), distanceEnemyToTask(Name,TaskId,Distance).

minDistance(Task,MinDistance) :- distanceEnemyToTask(_,Task,MinDistance), #min{D: distanceEnemyToTask(_,Task,D)} = MinDistance.

nearestEnemyToTask(Task,Enemy,Distance) :- distanceEnemyToTask(Enemy,Task,Distance), minDistance(Task,Distance).