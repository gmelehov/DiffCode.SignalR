
globalThis.SRHUBPAGE.hub = new signalR.HubConnectionBuilder().withUrl("/SRHub").withAutomaticReconnect().build();
globalThis.SRHUBPAGE.init();