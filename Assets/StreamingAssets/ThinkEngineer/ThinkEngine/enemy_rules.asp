% =================================================================================================================================
%  REGOLE:
nTasks(N) :- #count{Id: task(Id,_,MessageState,_,_,_,_,_,_), MessageState="Pending"} = N.
hasActiveTask :- task(_,_,_,_,_,_,_,Name,_), self(_,Name,_,_,_,_,_,_,_).

% Controlla se sei in stato di chasing.
isInChaseState :- self(_,_,_,_,_,_,"ChaseState",_,_).
hasActiveTask :- isInChaseState.

{takeTask(TaskId,Sender,MessageState,X,Y,Z,TaskType) :
    task(TaskId,Sender,MessageState,X,Y,Z,TaskType,AssignedTo,IsTaken),
    MessageState="Pending", AssignedTo="null", IsTaken=false,
    not hasActiveTask
} <= 1.

% =================================================================================================================================

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
% VINCOLI:
% Distanza: -------------------------------
% Se non sono tra i primi 4 più vicini ad un dato task, non lo prendo.
:- takeTask(TaskId,_,_,_,_,_,_), nEnemiesNearerToTask(SelfName,TaskId,N), self(_,SelfName,_,_,_,_,_,_,_), N > 4.

% Se sono colui che ha pubblicato il task, non posso prenderlo.
:- takeTask(TaskId,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_), task(TaskId,Name,_,_,_,_,_,_,_).

% Batteria: --------------------------------
% Non prendere task se hai batteria insufficiente.
:- takeTask(TaskId,_,_,_,_,_,_), self(_,SelfName,_,_,_,_,_,Battery,_), selfMinBattery(MinBattery), Battery<=MinBattery.


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