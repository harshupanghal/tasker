using Microsoft.AspNetCore.Components;
using Tasker.Web.Services;

namespace Tasker.Web.Components.Layout;
public partial class MainLayout : LayoutComponentBase
    {
    [Inject] private UserSessionService Session { get; set; } = default!;

    protected override async Task OnInitializedAsync()
        {
        await Session.LoadSessionAsync();
        }
    }
