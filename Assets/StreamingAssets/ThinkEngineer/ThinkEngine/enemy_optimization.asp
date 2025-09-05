% RICORDA: Per identificare i nemici nelle regole usa il nome, l'id a volte dà problemi!

% VINCOLI:
% Distanza: -------------------------------
% Non prendere task troppo lontani (oltre 100 unità).
%:- takeTask(TaskId,_,_,_,_,_,_), self(_,SelfName,_,_,_,_,_,_,_), distanceSelfToTask(TaskId,Distance), Distance > 100.
% Se non sono tra i primi 5 più vicini ad un dato task, non lo prendo.
:- takeTask(TaskId,_,_,_,_,_,_), nEnemiesNearerToTask(SelfName,TaskId,N), self(_,SelfName,_,_,_,_,_,_,_), N > 5.

% Se sono colui che ha pubblicato il task, non posso prenderlo.
:- takeTask(TaskId,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_), task(TaskId,Name,_,_,_,_,_,_,_).

% Batteria: --------------------------------
% Non prendere task se hai batteria insufficiente.
:- takeTask(TaskId,_,_,_,_,_,_), self(_,SelfName,_,_,_,_,_,Battery,_), selfMinBattery(MinBattery), Battery<=MinBattery.

% =================================================================================================================================
% PENALIZZAZIONI:
% Penalizzazione se non si prende nessun task quando ce n'è almeno uno (del tipo ottimo) disponibile:
% Penalizzazione EcoSentinel --------------------------------
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"investigation")}=0,
    self(_,Name,"EcoSentinel",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"investigation",_,_),
    distanceSelfToTask(TaskId,D). [D@3,TaskId,Name]

% Penalizzazione OverrideStalker --------------------------------
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"reinforcement")}=0,
    self(_,Name,"OverrideStalker",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"reinforcement",_,_),
    distanceSelfToTask(TaskId,D). [D@3,TaskId,Name]

% Penalizzazione CloseRangeEnforcer --------------------------------
% Reinforcement con priorità più alta
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"reinforcement")}=0,
    self(_,Name,"CloseRangeEnforcer",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"reinforcement",_,_),
    distanceSelfToTask(TaskId,D). [D@3,TaskId,Name]

% Altri task con peso minore
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"investigation")}=0,
    self(_,Name,"CloseRangeEnforcer",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"investigation",_,_),
    distanceSelfToTask(TaskId,D). [D@2,TaskId,Name]

% =================================================================================================================================
% ACTIONS:
applyAction(1,"TakeTask") :- takeTask(_,_,_,_,_,_,_).
actionArgument(1,"MessageIndex",TaskId) :- takeTask(TaskId,_,_,_,_,_,_).
actionArgument(1,"EnemyName",Name) :- takeTask(_,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_).

#show applyAction/2.
#show actionArgument/3.