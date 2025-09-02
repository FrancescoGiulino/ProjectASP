% RICORDA: Per identificare i nemici nelle regole usa il nome, l'id a volte dà problemi!

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
%#show task/9.
%#show takeTask/7.
%#show hasActiveTask/0.
#show distanceSelfToTask/2.
#show distanceEnemyToTask/3.
#show applyAction/2.
#show actionArgument/3.