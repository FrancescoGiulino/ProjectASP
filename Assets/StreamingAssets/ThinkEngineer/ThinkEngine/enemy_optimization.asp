% RICORDA: Per identificare i nemici nelle regole usa il nome, l'id a volte dà problemi!

% VINCOLI:
% Distanza:
% Non prendere task troppo lontani (oltre 80 unità).
:- takeTask(TaskId,_,_,_,_,_,_), self(_,SelfName,_,_,_,_,_,_,_), distanceSelfToTask(TaskId,Distance), Distance > 80.
% Se non sono nel "podio" dei più vicini ad un dato task, non lo prendo.
:- takeTask(TaskId,_,_,_,_,_,_), nEnemiesNearerToTask(SelfName,TaskId,N), self(_,SelfName,_,_,_,_,_,_,_), N > 3.

% =================================================================================================================================
% PENALIZZAZIONI:
% Penalizzazione se non si prende nessun task quando ce n'è almeno uno (del tipo ottimo) disponibile:
:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"investigation")}=0,
    self(_,Name,"EcoSentinel",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"investigation",_,_). [1@3]

:~ #count{TaskId: takeTask(TaskId,_,"Pending",_,_,_,"reinforcement")}=0,
    self(_,Name,"OverrideStalker",_,_,_,_,_,_),
    task(_,_,"Pending",_,_,_,"reinforcement",_,_). [1@3]

% =================================================================================================================================
% ACTIONS:
    applyAction(1,"TakeTask") :- takeTask(_,_,_,_,_,_,_).
    actionArgument(1,"MessageIndex",TaskId) :- takeTask(TaskId,_,_,_,_,_,_).
    actionArgument(1,"EnemyName",Name) :- takeTask(_,_,_,_,_,_,_), self(_,Name,_,_,_,_,_,_,_).

#show applyAction/2.
#show actionArgument/3.