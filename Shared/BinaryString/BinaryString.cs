using System.Text;

namespace AoC.Shared.BinaryString;

public class BinaryString
{
    private string _value;
    
    public BinaryString(string value)
    {
        _value = value;
    }
    
    public BinaryString ApplyMask(string mask, char maskChar)
    {
        if (mask.Length != _value.Length)
        {
            throw new ArgumentException("Mask length must match value length");
        }
        
        var newValue = new StringBuilder();
        
        for (var i = 0; i < mask.Length; i++)
        {
            newValue.Append(mask[i] == maskChar ? _value[i] : mask[i]);
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public long ToLong()
    {
        return Convert.ToInt64(_value, 2);
    }
    
    public override string ToString()
    {
        return _value;
    }
    
    public static BinaryString operator +(BinaryString a, BinaryString b)
    {
        if (a._value.Length != b._value.Length)
        {
            throw new ArgumentException("Binary strings must be the same length");
        }
        
        var newValue = new StringBuilder();
        
        var carry = '0';
        
        for (var i = a._value.Length - 1; i >= 0; i--)
        {
            var sum = a._value[i] + b._value[i] + carry - '0' - '0';
            
            newValue.Insert(0, sum % 2 == 1 ? '1' : '0');
            
            carry = sum > 1 ? '1' : '0';
        }
        
        if (carry == '1')
        {
            newValue.Insert(0, '1');
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public static BinaryString operator &(BinaryString a, BinaryString b)
    {
        if (a._value.Length != b._value.Length)
        {
            throw new ArgumentException("Binary strings must be the same length");
        }
        
        var newValue = new StringBuilder();
        
        for (var i = 0; i < a._value.Length; i++)
        {
            newValue.Append(a._value[i] == '1' && b._value[i] == '1' ? '1' : '0');
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public static BinaryString operator |(BinaryString a, BinaryString b)
    {
        if (a._value.Length != b._value.Length)
        {
            throw new ArgumentException("Binary strings must be the same length");
        }
        
        var newValue = new StringBuilder();
        
        for (var i = 0; i < a._value.Length; i++)
        {
            newValue.Append(a._value[i] == '1' || b._value[i] == '1' ? '1' : '0');
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public static BinaryString operator ^(BinaryString a, BinaryString b)
    {
        if (a._value.Length != b._value.Length)
        {
            throw new ArgumentException("Binary strings must be the same length");
        }
        
        var newValue = new StringBuilder();
        
        for (var i = 0; i < a._value.Length; i++)
        {
            newValue.Append(a._value[i] == b._value[i] ? '0' : '1');
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public static BinaryString operator ~(BinaryString a)
    {
        var newValue = new StringBuilder();
        
        for (var i = 0; i < a._value.Length; i++)
        {
            newValue.Append(a._value[i] == '1' ? '0' : '1');
        }
        
        return new BinaryString(newValue.ToString());
    }
    
    public static BinaryString operator <<(BinaryString a, int shift)
    {
        var newValue = new StringBuilder(a._value);
        
        for (var i = 0; i < shift; i++)
        {
            newValue.Append('0');
        }
        
        return new BinaryString(newValue.ToString()[shift..]);
    }
    
    public static BinaryString operator >>(BinaryString a, int shift)
    {
        var newValue = new StringBuilder();
        
        for (var i = 0; i < shift; i++)
        {
            newValue.Append('0');
        }
        
        newValue.Append(a._value);
        
        return new BinaryString(newValue.ToString()[..a._value.Length]);
    }
}