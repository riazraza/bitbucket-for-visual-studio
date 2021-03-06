﻿using System;
using System.ComponentModel.Composition;
using GitClientVS.Contracts.Events;
using GitClientVS.Contracts.Interfaces.Services;
using GitClientVS.Contracts.Interfaces.Views;
using GitClientVS.Contracts.Models.GitClientModels;
using GitClientVS.TeamFoundation.TeamFoundation;
using GitClientVS.UI;
using Microsoft.TeamFoundation.Controls;

namespace GitClientVS.TeamFoundation.Sections
{
    [TeamExplorerSection(Id, TeamExplorerPageIds.GitCommits, 50)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PublishSection : TeamExplorerBaseSection
    {
        private readonly IAppServiceProvider _appServiceProvider;
        private readonly IUserInformationService _userInformationService;
        private readonly IEventAggregatorService _eventAggregator;
        private IDisposable _obs;
        private const string Id = "8a950046-66b6-4607-9038-4d0b7eb8ab96";

        [ImportingConstructor]
        public PublishSection(
            IPublishSectionView view,
            IAppServiceProvider appServiceProvider,
            IGitClientService gitClientService,
            IGitWatcher gitWatcher,
            IUserInformationService userInformationService,
            IEventAggregatorService eventAggregator
            ) : base(view)
        {
            _appServiceProvider = appServiceProvider;
            _userInformationService = userInformationService;
            _eventAggregator = eventAggregator;
            Title = $"{Resources.PublishSectionTitle} to {gitClientService.Origin}";
            IsVisible = IsGitLocalRepoAndLoggedIn(gitWatcher.ActiveRepo);
            _obs = _eventAggregator.GetEvent<ActiveRepositoryChangedEvent>().Subscribe(x => IsVisible = IsGitLocalRepoAndLoggedIn(x.ActiveRepository));
        }

        private bool IsGitLocalRepoAndLoggedIn(GitRemoteRepository repo)
        {
            return _userInformationService.ConnectionData.IsLoggedIn && (repo != null && string.IsNullOrEmpty(repo.CloneUrl));
        }

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            _appServiceProvider.GitServiceProvider = ServiceProvider = e.ServiceProvider;
            base.Initialize(sender, e);
        }

        public override void Dispose()
        {
            base.Dispose();
            _obs.Dispose();
        }
    }
}
