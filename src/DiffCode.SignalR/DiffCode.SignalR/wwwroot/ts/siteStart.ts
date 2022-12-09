/// <reference types="@microsoft/signalr/dist/esm/index" />
import { IConnection } from "./interfaces";








export class SRHubPage
{
  constructor()
  {
    
  }







  public hub: signalR.HubConnection;


  public started: boolean = false;


  /** Код клиентского подключения */
  public cid: string

  /** Код предыдущего клиентского подключения */
  public prevcid: string

  /** Имя пользователя, открывшего это подключение */
  public name: string

  /** IP-адрес пользователя, открывшего это подключение */
  public ip: string;


  public path: string;


  public uagent: string









  /** Запускает хаб SignalR */
  start()
  {
    this.hub.start().then(function ()
    {

    }).catch(function (err)
    {

      return console.error(err.toString());
    });
  }


  openWindow(newurl: string, width: number = 800, height: number = 500)
  {
    window.open(newurl, "_blank", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=yes,width=" + width + ",height=" + height);
  }



  init()
  {
    
    /**
     * Обработчик события RcvUpdatedPath.
     * Сохраняет код текущего (уже записанного в БД) подключения в Session Storage браузера.
     * После сохранения обработчик вызывает серверный метод GetMyConnection 
     * для получения всего объекта Connection.
     */
    this.hub.on('RcvUpdatedPath', (cid, path, uagent, pcid) =>
    {
      window.sessionStorage.setItem('cid', cid);
      window.localStorage.setItem('cid', cid);
      window.localStorage.setItem('prevcid', pcid);
      this.pushCidToLocalStorage(cid);
      this.path = path;
      this.uagent = uagent;
      this.prevcid = pcid;

      this.hub.invoke('GetMyConnection');
    });



    this.hub.on('EnsureActualCid', (cid) =>
    {
      this.cid = cid;
      window.sessionStorage.setItem('cid', cid);
      window.localStorage.setItem('cid', cid);
      this.pushCidToLocalStorage(cid);
    });


    /**
     * Обработчик события RcvConnectedEvent.
     * Сохраняет код текущего (только что созданного) подключения в свойство cid этого объекта.
     * Сохранять его в Session Storage браузера сейчас нельзя, т.к. в данный момент в нем
     * может содержаться код предыдущего подключения.
     * После сохранения обработчик вызывает серверный метод UpdateCallerInfo.
     */
    this.hub.on('RcvConnectedEvent', (cid) =>
    {
      this.cid = cid;
      this.started = true;
      this.UpdateCallerInfo();
    });


    /**
     * Обработчик события RcvMyConnection.
     * Получает с сервера объект текущего подключения и сохраняет его в этом объекте.
     * На этом обработка нового подключения к сайту завершена.
     */
    this.hub.on('RcvMyConnection', (conn: IConnection) =>
    {
      console.clear();
      this.receiveMyConn(conn);
    });


    /**
     * Обработчик события UpdMyConnection.
     * Получает с сервера объект текущего подключения и сохраняет его в этом объекте.
     * Событие можно инициировать на стороне сервера в любое время при необходимости,
     * не разрывая текущего подключения.
     */
    this.hub.on('UpdMyConnection', (conn: IConnection) =>
    {
      this.updateMyConn(conn);
    });




    this.hub.on('BrowseToUrl', function (newurl, opennew)
    {
      if (newurl !== null && newurl !== undefined)
      {
        window.open(newurl, opennew === true ? '_blank' : '_self', '');
      };
    });






    this.start();
  }





  pushCidToLocalStorage(cid: string)
  {
    let cids = JSON.parse(window.localStorage.getItem('cids')) as string[] || [];
    if (!cids.some(s => s == cid))
    {
      cids.push(cid);
      window.localStorage.setItem('cids', JSON.stringify(cids));
    }
  }





  /**
   * Обновляет сведения о клиентском подключении, используя данные, полученные с сервера.
   * @param conn
   */
  receiveMyConn(conn: IConnection)
  {
    this.cid = conn.Cid
    this.decomposeMyConn(conn)
  }

  /**
   * Обновляет сведения о клиентском подключении, используя данные, полученные с сервера.
   * @param conn
   */
  updateMyConn(conn: IConnection)
  {
    if (this.cid === conn.Cid)
    {
      this.decomposeMyConn(conn)
    };
  }


  decomposeMyConn(conn: IConnection)
  {
    this.ip = conn.IP;
    this.prevcid = conn.PrevCid;
    this.name = conn.Caller.UserName;
  }










  RewriteActualCid()
  {
    this.hub.invoke('RewriteActualCid');
  }

  /**
   * Вызов серверного метода UpdateCallerInfo, в который передаются сведения
   * о коде подключения из Session Storage браузера 
   * (= null, если это первое подключение пользователя к сайту, иначе = код предыдущего подключения),
   * строка User-Agent и данные о ширине и высоте окна браузера.
   * После выполнения серверного метода из него будет вызвано событие RcvUpdatedPath.
   */
  UpdateCallerInfo()
  {
    let pcid = window.sessionStorage.getItem('cid');
    let uagent = window.navigator.userAgent;
    this.hub.invoke('UpdateCallerInfo', window.location.pathname, uagent, pcid, window.screen.width, window.screen.height);
  }

  UpdateMyConnection()
  {
    this.hub.invoke('UpdateMyConnection');
  }


}





globalThis.SRHUBPAGE = new SRHubPage();