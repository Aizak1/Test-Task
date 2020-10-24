using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

class EnergyCompany
{
    static void Main(string[] args)
    {
        EnergyCompany companyManager = new EnergyCompany();
        Client[] clients =
        {
            new RegularClient(140),
            new RegularClient(30),
            new RegularClient(200),
            new PreferentialClient1(70),
            new PreferentialClient1(20),
            new PreferentialClient2(150),
            new LimitedClient(260),
            new PreferentialClient1(70),
            new PreferentialClient2(320),
            new LimitedClient(190),
            new LimitedClient(130),
            new PreferentialClient2(80)

        };
        #region = Сортировка По количеству затраченной энергии по убыванию
        clients = companyManager.SortByDescendingByEnergyConsumed(clients);
        Console.WriteLine("Array is sorted by consumed Energy");
        
        foreach (var item in clients)
        {
            Console.WriteLine(item.EnergyAmount);
        }
        #endregion

        #region = Cортировка по величине оплаты клиента по возрастанию
        clients = companyManager.SortByEnergyFullPrice(clients);
        Console.WriteLine();
        Console.WriteLine("Array is sorted by energy full Price");
        foreach (var item in clients)
        {
            Console.WriteLine(item.EnergyFullPrice);
        }
        #endregion

        #region = Сортировка по статусу Клиента
        clients = companyManager.SortByClientStatus(clients);
        Console.WriteLine();
        Console.WriteLine("Array is Sorted by client status");
        foreach (var item in clients)
        {
            Console.WriteLine(item.Status);
        }
        #endregion

        #region = Сумма оплаты всех клиетов за потребленную энергию
        Console.WriteLine();
        Console.WriteLine($"Total sum From all Clients = {companyManager.TotalPaymentFromAllClients(clients)}");
        #endregion

        #region = Общий размер всех льгот
        Console.WriteLine();
        Console.WriteLine($"Sum of All Facilites = {companyManager.SumOfAllFacilities(clients)}");
        #endregion
    }

Client[] SortByDescendingByEnergyConsumed(Client[] clients)
    {
     return clients.OrderByDescending(client => client.EnergyAmount).ToArray();
    }    
Client[] SortByEnergyFullPrice(Client[] clients)
    {
        return clients.OrderBy(client => client.EnergyFullPrice).ToArray();
    }
Client[] SortByClientStatus(Client[] clients)
    {
        return clients.OrderBy(client => client.Status).ToArray();
    }
double TotalPaymentFromAllClients(Client[] clients)
   {
        return clients.Select(client => client.EnergyFullPrice).Sum();
   }
double SumOfAllFacilities(Client[] clients)
{
        return clients.Select(client => client.EnergyAmount * client.CostPerEnergyUE).Sum() - clients.Select(client => client.EnergyFullPrice).Sum();
}

}

public enum ClientStatus
{
    Regular,
    Limited,
    PreferentialClient1,
    PreferentialClient2

}
class Client
{
   
    private double _energyAmount;

    protected ClientStatus _status;
    protected const double _costPerEnergyUE = 15;
    protected double _tarifCoefficent;
    protected double _energyFullPrice;
    public double EnergyFullPrice => _energyFullPrice;

    public double EnergyAmount => _energyAmount;

    public double CostPerEnergyUE => _costPerEnergyUE;

    public ClientStatus Status => _status;

    public Client(double _energyAmount)
    {
        this._tarifCoefficent = 1;
        this._energyAmount = _energyAmount;
  
    }


    protected virtual double CalculateFullPrice(double _energyAmount,double _costPerEnergyUE,double _tarifCoefficent)
    {
        return _energyAmount*_costPerEnergyUE*_tarifCoefficent;
    }
 
}


class RegularClient : Client
{
    public RegularClient(double _energyAmount) : base(_energyAmount)
    {
        _status = ClientStatus.Regular;
        _energyFullPrice = CalculateFullPrice(_energyAmount, _costPerEnergyUE, _tarifCoefficent);
    }
}
class LimitedClient : Client
{
   private double _limit = 150;

    public LimitedClient(double _energyAmount) :base(_energyAmount)
    {
        _status = ClientStatus.Limited;
        _tarifCoefficent = 1/3f;
        _energyFullPrice = CalculateFullPrice(_energyAmount, _costPerEnergyUE, _tarifCoefficent);
    }
   
    
   
    protected override double CalculateFullPrice(double _energyAmount, double _costPerEnergyUE, double _tarifCoefficent)
    {
       
         
        return _energyAmount<=_limit ? _energyAmount*_costPerEnergyUE  :  
            _limit*_costPerEnergyUE + (_energyAmount-_limit) * (_costPerEnergyUE + _costPerEnergyUE * _tarifCoefficent);
    }


}

class PreferentialClient1 : Client
{
    public PreferentialClient1(double _energyAmount) : base(_energyAmount)
    {
        _status = ClientStatus.PreferentialClient1;
        _tarifCoefficent = 2/3f;
        _energyFullPrice = CalculateFullPrice(_energyAmount, _costPerEnergyUE, _tarifCoefficent);
    }
   
}

class PreferentialClient2 : Client
{
    private double _amountOfFreeEnergyUe = 50;
    public PreferentialClient2(double _energyAmount) : base(_energyAmount)
    {
        _status = ClientStatus.PreferentialClient2;
        _energyFullPrice = CalculateFullPrice(_energyAmount, _costPerEnergyUE, _tarifCoefficent);
    }

    protected override double CalculateFullPrice(double _energyAmount, double _costPerEnergyUE, double _tarifCoefficent)
    {

        return _energyAmount<=_amountOfFreeEnergyUe? 0 : 
            base.CalculateFullPrice(_energyAmount-_amountOfFreeEnergyUe, _costPerEnergyUE, _tarifCoefficent);
    }
}














