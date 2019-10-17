program WeDoScan;

uses
  Vcl.Forms,
  main in 'main.pas' {fmMain},
  devinfo in 'devinfo.pas' {fmDevInfo};

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfmMain, fmMain);
  Application.Run;
end.
