﻿using _ = Atata.Tests.WaitingPage;

namespace Atata.Tests
{
    [Url("Waiting.html")]
    [VerifyTitle]
    [VerifyH1]
    public class WaitingPage : Page<_>
    {
        public enum WaitKind
        {
            None,
            WaitForElementHiddenOrMissing,
            WaitForMissing,
            VerifyMissing
        }

        [Term("Wait")]
        public ButtonDelegate<_> ButtonWithoutWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "processing-block")]
        public ButtonDelegate<_> ButtonWithMissingOrHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "processing-block", Until.VisibleThenHidden)]
        public ButtonDelegate<_> ButtonWithVisibleAndHiddenWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Css, ".nonexistent-block", Until.VisibleThenMissing, PresenceTimeout = 2, ThrowOnPresenceFailure = false)]
        public ButtonDelegate<_> ButtonWithVisibleAndMissingWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Css, ".nonexistent-block", Until.VisibleThenMissing, PresenceTimeout = 1)]
        public ButtonDelegate<_> ButtonWithVisibleAndMissingNonExistentWait { get; private set; }

        [Term("Wait")]
        [WaitForElement(WaitBy.Class, "result-block", Until.HiddenThenVisible)]
        public ButtonDelegate<_> ButtonWithHiddenAndVisibleWait { get; private set; }

        [FindByClass("result-block")]
        public Text<_> Result { get; private set; }

        [IgnoreInit]
        public Link<WaitingOnInitPage, _> GoToWaitingOnInitPage { get; private set; }

        [IgnoreInit]
        public Button<WaitingOnInitPage, _> WaitAndGoToWaitingOnInitPage { get; private set; }

        [FindByClass]
        public Control<_> NavigatingBlock { get; private set; }

        public Button<_> WaitAndUpdateValue { get; private set; }

        [FindById]
        public Text<_> ValueBlock { get; private set; }

        [FindByClass("value-container")]
        public ValueContainerControl ValueContainer { get; private set; }

        public WaitingOnInitPage.WaitKind NavigatingPageWaitKind { get; set; }

        public WaitKind NavigatingWaitKind { get; set; }

        [FindById("missing-id")]
        public Control<_> MissingControl { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            GoToWaitingOnInitPage = Controls.CreateLink(
                nameof(GoToWaitingOnInitPage),
                () => new WaitingOnInitPage { OnInitWaitKind = NavigatingPageWaitKind },
                new TermAttribute("Go to WaitingOnInit page"));

            WaitAndGoToWaitingOnInitPage = Controls.CreateButton(
                nameof(WaitAndGoToWaitingOnInitPage),
                () => new WaitingOnInitPage { OnInitWaitKind = NavigatingPageWaitKind },
                new TermAttribute("Wait and go to WaitingOnInit page"));

            if (NavigatingWaitKind == WaitKind.WaitForElementHiddenOrMissing)
                Triggers.Add(new WaitForElementAttribute(WaitBy.Class, "navigating-block", Until.MissingOrHidden, TriggerEvents.DeInit));
            else if (NavigatingWaitKind == WaitKind.WaitForMissing)
                NavigatingBlock.Triggers.Add(new WaitForAttribute(Until.Missing, TriggerEvents.DeInit));
            else if (NavigatingWaitKind == WaitKind.VerifyMissing)
                NavigatingBlock.Triggers.Add(new VerifyMissingAttribute(TriggerEvents.DeInit));
        }

        public class ValueContainerControl : Control<_>
        {
            [FindById]
            public Text<_> ValueBlock { get; private set; }
        }
    }
}
