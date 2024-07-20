using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salida : PlacedObject
{
 

  public void cargarCaja(Caja caja)
  {
    caja.DestroySelf();
  }


}
