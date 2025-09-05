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

%minDistance(Task,MinDistance) :- distanceEnemyToTask(_,Task,MinDistance), #min{D: distanceEnemyToTask(_,Task,D)} = MinDistance.
%nearestEnemyToTask(Task,Enemy,Distance) :- distanceEnemyToTask(Enemy,Task,Distance), minDistance(Task,Distance).

% Un nemico NearEnemy è più vicino di FarEnemy al task
enemyNearerToTask(NearEnemy,FarEnemy,Task) :-
    distanceEnemyToTask(NearEnemy,Task,D1),
    distanceEnemyToTask(FarEnemy,Task,D2),
    NearEnemy != FarEnemy,
    D1 < D2.

% Conta quanti nemici sono più vicini di Enemy al task
nEnemiesNearerToTask(Enemy,Task,N) :-
    distanceEnemyToTask(Enemy,Task,_),
    N = #count{NearEnemy : enemyNearerToTask(NearEnemy,Enemy,Task)}.

% =================================================================================================================================
% Controlla se sei in stato di chasing.
%self(SelfId, Name, ReasoningStyle, X, Y, Z, CurrentStateName, CurrentHealth, IsDead)
isInChaseState :- self(_,_,_,_,_,_,"ChaseState",_,_).

#show self/9.
#show enemies/9.
#show task/9.
#show takeTask/7.
#show hasActiveTask/0.
#show isInChaseState/0.
%#show distanceSelfToTask/2.
#show distanceEnemyToTask/3.
%#show minDistance/2.
%#show nearestEnemyToTask/3.
%#show enemyNearerToTask/3.
%#show nEnemiesNearerToTask/3.

#show selfMinBattery/1.
#show enemyMinBattery/2.