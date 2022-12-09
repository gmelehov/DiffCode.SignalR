


export interface IConnection
{

  Id: number;

  Cid: string;

  Group: string;

  UserAgent: string;
  
  IP?: string;

  StartedOn: Date;

  ClosedOn?: Date | null;

  IsActive: boolean;

  IsForcedClosed: boolean;
  
  CallerId: number;

  Caller: IUser;

  PrevId?: number | null;

  PrevCid: string;

  ConnectReason: ConnectReason;

  DisconnectReason: DisconnectReason;

}







export interface IUser
{

  Id: number;

  UserName: string;

  Email: string;

  LastOnlineOn?: Date | null;

  IsOnline: boolean;

  ActiveConns: number;

}












export enum DisconnectReason
{

  NONE = 0,

  ON_REFRESH = 1,

  ON_BROWSE = 2,

  ON_RECONNECT = 3,

  ON_ERROR = 4,

  ON_OTHER = 10,

}








export enum ConnectReason
{

  NONE = 0,

  AFTER_REFRESH = 1,

  AFTER_BROWSE = 2,

  AFTER_RECONNECT = 3,

  AFTER_ERROR = 4,

  AFTER_OTHER = 10,

}