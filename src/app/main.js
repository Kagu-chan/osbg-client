const electron = require("electron");
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;

let win;

function createWindow(){
  win = new BrowserWindow({width:1024, height:720});
  win.loadURL(`file://${__dirname}/index.html`);
  win.on('closed', () => {
    win = null;
  });

  win.webContents.openDevTools();
}

app.on('ready', createWindow);
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});
app.on('activate', () => {
  if (win === null) {
    createWindow();
  }
});