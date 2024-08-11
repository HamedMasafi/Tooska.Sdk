namespace Tooska.Payment;

public class GatewaysPool<T> where T : AbstractTransaction
{
    private readonly Dictionary<string, AbstractPaymentGateway<T>> _gateways = new();

    public void Add(AbstractPaymentGateway<T> gateway)
    {
        var key = gateway.GetType().Name;
        if (_gateways.ContainsKey(key))
            return;
        
        _gateways.Add(key, gateway);
    }

    public AbstractPaymentGateway<T> Gateway(int index)
    {
        return _gateways.ElementAt(index).Value;
    }
    
    public AbstractPaymentGateway<T> Gateway(string name)
    {
        return _gateways[name];
    }

    public int Count => _gateways.Count;
    
    
}