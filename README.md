Asp.netMVCPatchExample
======================

An example of how to implement a patch method, similar to the Patch from Delta of oData...

I found it incredibly annoying that oDatas patch was case sensitive, thus i decided to do away with the whole oData part, since it is a huge library that we are abusing....<br></p>
<p>
I decided to implement my own patch method, since that is the muscle that we are actually lacking. I created the following abstract class:
</p>

```csharp
    public abstract class MyModel
    {
        public void Patch(Object u)
        {
            var props = from p in this.GetType().GetProperties()
                        let attr = p.GetCustomAttribute(typeof(NotPatchableAttribute))
                        where attr == null
                        select p;
            foreach (var prop in props)
            {
                var val = prop.GetValue(this, null);
                if (val != null)
                    prop.SetValue(u, val);
            }
        }
    }
```
<p>
Then i make all my model classes inherit from *MyModel*. note the line where i use *let*, i will excplain that later. So now you can remove the Delta<Entry> from you controller action, and just make it Entry again, as with the put method. e.g.
</p>

```csharp
    public IHttpActionResult PatchUser(int id, Entry newEntry)
```

You can still use the patch method the way you used to:

```csharp
    var entry = dbContext.Entries.SingleOrDefault(p => p.ID == id);
    newEntry.Patch(entry);
    dbContext.SaveChanges();
```

Now, let's get back to the line

```csharp
    let attr = p.GetCustomAttribute(typeof(NotPatchableAttribute))
```

I found it a security risk that just any property would be able to be updated with a patch request. For example, you might now want the an ID to be changeble by the patch. I created a custom attribute to decorate my properties with. the NotPatchable attribute:

```csharp
    public class NotPatchableAttribute : Attribute {}
```

You can use it just like any other attribute:

```csharp
    public class User : MyModel
    {
        [NotPatchable]
        public int ID { get; set; }
        [NotPatchable]
        public bool Deleted { get; set; }
        public string FirstName { get; set; }
    }
```

This in this call the Deleted and ID properties cannot be changed though the patch method.
<p>
I hope this solve it for you as well. Do not hesitate to leave a comment if you have any questions.
</p>
<p>
I added a screenshot of me inspecting the props in a new mvc 5 project. As you can see the Result view is populated with the Title and ShortDescription.
</p>
![Example of inspecting the props][1]


  [1]: http://i.stack.imgur.com/laSxG.png
