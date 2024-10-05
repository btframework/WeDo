////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//   Wireless Communication Library 7                                         //
//                                                                            //
//   Copyright (C) 2006-2020 Mike Petrichenko                                 //
//                           Soft Service Company                             //
//                           All Rights Reserved                              //
//                                                                            //
//   http://www.btframework.com                                               //
//                                                                            //
//   support@btframework.com                                                  //
//   shop@btframework.com                                                     //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

{$DEFINITIONINFO ON}
{$REFERENCEINFO ON}

{$IFNDEF _WCL_DEBUG_}
  {$ASSERTIONS OFF}
  {$DEBUGINFO OFF}
  {$LOCALSYMBOLS OFF}
  {$IOCHECKS OFF}
  {$OVERFLOWCHECKS OFF}
  {$OPTIMIZATION OFF}
{$ELSE}
  {$ASSERTIONS ON}
  {$DEBUGINFO ON}
  {$LOCALSYMBOLS ON}
  {$IOCHECKS ON}
  {.$OVERFLOWCHECKS ON}
  {$OVERFLOWCHECKS OFF}
  {$OPTIMIZATION ON}
{$ENDIF}

{$STACKFRAMES OFF}
{$SAFEDIVIDE OFF}
{$IFDEF VER130}
  {$ALIGN ON}
{$ELSE}
  {$ALIGN 8}
{$ENDIF}
{$RANGECHECKS OFF}
{$VARSTRINGCHECKS OFF}
{$BOOLEVAL OFF}
{$EXTENDEDSYNTAX ON}
{$TYPEDADDRESS OFF}
{$OPENSTRINGS OFF}
{$LONGSTRINGS ON}
{$WRITEABLECONST OFF}
{$MINENUMSIZE 4}

{$IFDEF BCB}
  {$ObjExportAll On}
{$ENDIF}

{$IFDEF VER300}
  {$DEFINE D6}
  {$DEFINE D7}
  {$DEFINE D2005}
  {$DEFINE D2006}
  {$DEFINE D2007}
  {$DEFINE D2009}
  {$DEFINE D2010}
  {$DEFINE DXE}
  {$DEFINE DXE2}
  {$DEFINE DXE3}
  {$DEFINE DXE4}
  {$DEFINE DXE5}
  {$DEFINE DXE6}
  {$DEFINE DXE7}
  {$DEFINE DXE8}
  {$DEFINE D10}
{$ENDIF}

{$IFDEF VER310}
  {$DEFINE D6}
  {$DEFINE D7}
  {$DEFINE D2005}
  {$DEFINE D2006}
  {$DEFINE D2007}
  {$DEFINE D2009}
  {$DEFINE D2010}
  {$DEFINE DXE}
  {$DEFINE DXE2}
  {$DEFINE DXE3}
  {$DEFINE DXE4}
  {$DEFINE DXE5}
  {$DEFINE DXE6}
  {$DEFINE DXE7}
  {$DEFINE DXE8}
  {$DEFINE D10}
  {$DEFINE D101}
{$ENDIF}

{$IFDEF VER320}
  {$DEFINE D6}
  {$DEFINE D7}
  {$DEFINE D2005}
  {$DEFINE D2006}
  {$DEFINE D2007}
  {$DEFINE D2009}
  {$DEFINE D2010}
  {$DEFINE DXE}
  {$DEFINE DXE2}
  {$DEFINE DXE3}
  {$DEFINE DXE4}
  {$DEFINE DXE5}
  {$DEFINE DXE6}
  {$DEFINE DXE7}
  {$DEFINE DXE8}
  {$DEFINE D10}
  {$DEFINE D101}
  {$DEFINE D102}
{$ENDIF}

{$IFDEF VER330}
  {$DEFINE D6}
  {$DEFINE D7}
  {$DEFINE D2005}
  {$DEFINE D2006}
  {$DEFINE D2007}
  {$DEFINE D2009}
  {$DEFINE D2010}
  {$DEFINE DXE}
  {$DEFINE DXE2}
  {$DEFINE DXE3}
  {$DEFINE DXE4}
  {$DEFINE DXE5}
  {$DEFINE DXE6}
  {$DEFINE DXE7}
  {$DEFINE DXE8}
  {$DEFINE D10}
  {$DEFINE D101}
  {$DEFINE D102}
  {$DEFINE D103}
{$ENDIF}

{$IFDEF VER340}
  {$DEFINE D6}
  {$DEFINE D7}
  {$DEFINE D2005}
  {$DEFINE D2006}
  {$DEFINE D2007}
  {$DEFINE D2009}
  {$DEFINE D2010}
  {$DEFINE DXE}
  {$DEFINE DXE2}
  {$DEFINE DXE3}
  {$DEFINE DXE4}
  {$DEFINE DXE5}
  {$DEFINE DXE6}
  {$DEFINE DXE7}
  {$DEFINE DXE8}
  {$DEFINE D10}
  {$DEFINE D101}
  {$DEFINE D102}
  {$DEFINE D103}
  {$DEFINE D104}
{$ENDIF}

{$IFNDEF D10}
  {$MESSAGE FATAL 'Unsupported Delphi or CBuilder version'}
{$ENDIF}
