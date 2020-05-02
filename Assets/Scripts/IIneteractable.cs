using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOperate
{
    bool InitOperation(Person person);
    void EndOperation(Person person);
}

public interface IPut
{
    bool put(Item item);
    bool CanTake(Item item);
    List<Ingredient> GetInputs();
}


public interface IGet
{
    Item get(Item item);
    List<Ingredient> GetOutputs();
    bool Has(Item item);
}

public interface IBuyable
{
    float GetPrice();
}
