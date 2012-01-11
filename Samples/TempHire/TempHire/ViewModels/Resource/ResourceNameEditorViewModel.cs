﻿using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Common.Dialog;
using Common.Errors;
using Common.Repositories;

namespace TempHire.ViewModels.Resource
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ResourceNameEditorViewModel : Screen, IDialogHostDelegate
    {
        private readonly IErrorHandler _errorHandler;
        private readonly IRepositoryManager<IResourceRepository> _repositoryManager;
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private IResourceRepository _repository;
        private Guid _resourceId;

        [ImportingConstructor]
        public ResourceNameEditorViewModel(IRepositoryManager<IResourceRepository> repositoryManager,
                                           IErrorHandler errorHandler)
        {
            _repositoryManager = repositoryManager;
            _errorHandler = errorHandler;
        }

        private IResourceRepository Repository
        {
            get { return _repository ?? (_repository = _repositoryManager.GetRepository(_resourceId)); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                OnCompleteChanged();
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                NotifyOfPropertyChange(() => MiddleName);
                OnCompleteChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                OnCompleteChanged();
            }
        }

        #region IDialogHostDelegate Members

        public bool IsComplete
        {
            get { return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName); }
        }

        public DialogResult DialogResult { get; set; }

        public event EventHandler CompleteChanged = delegate { };

        #endregion

        public ResourceNameEditorViewModel Start(Guid resourceId)
        {
            _resourceId = resourceId;
            Repository.GetResourceAsync(_resourceId,
                                        result =>
                                            {
                                                FirstName = result.FirstName;
                                                MiddleName = result.MiddleName;
                                                LastName = result.LastName;
                                            },
                                        _errorHandler.HandleError);
            return this;
        }

        public override void CanClose(Action<bool> callback)
        {
            if (DialogResult != DialogResult.Cancel)
            {
                callback(IsComplete);
            }
            else
                base.CanClose(callback);
        }

        private void OnCompleteChanged()
        {
            CompleteChanged(this, EventArgs.Empty);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
                _repository = null;
        }
    }
}