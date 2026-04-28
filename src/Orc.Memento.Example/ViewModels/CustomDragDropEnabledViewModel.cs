namespace Orc.Memento.Example.ViewModels;

using System;
using System.Windows;
using Catel.Logging;
using Catel.MVVM;
using GongSolutions.Wpf.DragDrop;
using Memento;
using Microsoft.Extensions.Logging;

// https://github.com/punker76/gong-wpf-dragdrop/wiki/Usage
public class CustomDragDropEnabledViewModel : ViewModelBase, IDragSource //, IDropTarget
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(CustomDragDropEnabledViewModel));

    protected readonly IMementoService _mementoService;

    private bool _operationSuccessFul;

    public CustomDragDropEnabledViewModel(IServiceProvider serviceProvider, IMementoService mementoService)
        : base(serviceProvider)
    {
        _mementoService = mementoService;
    }

    // dd:DragDrop.DragHandler="{Binding}"

    bool IDragSource.CanStartDrag(IDragInfo dragInfo)
    {
        Logger.LogInformation("CanStartDrag");
        return true;
    }

    void IDragSource.DragCancelled()
    {
        Logger.LogInformation("DragCancelled");
        _operationSuccessFul = false;
    }

    void IDragSource.DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
    {
        // will always called even if operation failed
        Logger.LogInformation("DragDropOperationFinished");

        _mementoService.EndBatch();
        if ( !_operationSuccessFul)
        {
            Logger.LogInformation("call undo");
            _mementoService.Undo();
        }
    }

    void IDragSource.Dropped(IDropInfo dropInfo)
    {
        Logger.LogInformation("Dropped");
        _operationSuccessFul = true;
    }

    void IDragSource.StartDrag(IDragInfo dragInfo)
    {
        Logger.LogInformation("StartDrag");
        _mementoService.BeginBatch("Drag & Drop");

        dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        dragInfo.Data = dragInfo.SourceItems;
    }

    bool IDragSource.TryCatchOccurredException(Exception exception)
    {
        Logger.LogInformation("TryCatchOccurredException");
        _operationSuccessFul = false;

        // we have handled the exception
        return true;
    }
}
