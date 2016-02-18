package validation;

import java.util.ArrayList;
import java.util.List;

public class UniqueIndex {
	private String name = "";
	
	private List<String> list = new ArrayList<String>();
	
	public UniqueIndex(String name){
		this.setName(name);
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getName() {
		return name;
	}
	
	public int getIndex(String s){
		for(int i = 0; i< list.size(); i++){
			if(list.get(i).equals(s)){
				return i+1;
			}
		}
		list.add(s);
		return list.size();
	}
}
