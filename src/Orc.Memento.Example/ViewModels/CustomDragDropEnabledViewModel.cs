namespace Orc.Memento.Example.ViewModels;

using System;
using System.Windows;
using Catel.Logging;
using Catel.MVVM;
using GongSolutions.Wpf.DragDrop;
using Memento;

// https://github.com/punker76/gong-wpf-dragdrop/wiki/Usage
public class CustomDragDropEnabledViewModel : ViewModelBase, IDragSource //, IDropTarget
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    protected readonly IMementoService _mementoService;

    private bool _operationSuccessFul;

    public CustomDragDropEnabledViewModel(IMementoService mementoService)
    {
        _mementoService = mementoService;
    }

    #region IDragSource
    // dd:DragDrop.DragHandler="{Binding}"

    bool IDragSource.CanStartDrag(IDragInfo dragInfo)
    {
        Log.Info("CanStartDrag");
        return true;
    }

    void IDragSource.DragCancelled()
    {
        Log.Info("DragCancelled");
        _operationSuccessFul = false;
    }

    void IDragSource.DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
    {
        // will always called even if operation failed
        Log.Info("DragDropOperationFinished");

        _mementoService.EndBatch();
        if ( !_operationSuccessFul)
        {
            Log.Info("call undo");
            _mementoService.Undo();
        }
    }

    void IDragSource.Dropped(IDropInfo dropInfo)
    {
        Log.Info("Dropped");
        _operationSuccessFul = true;
    }

    void IDragSource.StartDrag(IDragInfo dragInfo)
    {
        Log.Info("StartDrag");
        _mementoService.BeginBatch("Drag & Drop");

        dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        dragInfo.Data = dragInfo.SourceItems;
    }

    bool IDragSource.TryCatchOccurredException(Exception exception)
    {
        Log.Info("TryCatchOccurredException");
        _operationSuccessFul = false;

        // we have handled the exception
        return true;
    }
    #endregion
}
